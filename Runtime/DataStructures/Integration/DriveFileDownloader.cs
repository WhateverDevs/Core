using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.DataStructures.Integration
{
    /// <summary>
    /// Credits to: https://gist.github.com/glockbender/4e95aafc0c544bf7e51c06c8d44e9854
    /// And: https://stackoverflow.com/questions/33713084/download-link-for-google-spreadsheets-csv-export-with-multiple-sheets
    /// </summary>
    public static class DriveFileDownloader
    {
        private const string GoogleDriveDomain = "drive.google.com";
        private const string GoogleDriveDomain2 = "https://drive.google.com";

        /// <summary>
        /// Retrieve a file from Google Drive and return its content.
        /// </summary>
        public static string GetTextFromUrl(string url)
        {
            string tempPath = Application.persistentDataPath + "/temp.tsv";

            if (File.Exists(tempPath)) File.Delete(tempPath);

            FileInfo file = DownloadFileFromURLToPath(url, tempPath);

            string result = File.ReadAllText(file.FullName);

            file.Delete();

            return result;
        }

        // Normal example: FileDownloader.DownloadFileFromURLToPath( "http://example.com/file/download/link", @"C:\file.txt" );
        // Drive example: FileDownloader.DownloadFileFromURLToPath( "http://drive.google.com/file/d/FILEID/view?usp=sharing", @"C:\file.txt" );
        // Download as Tsv example: https://docs.google.com/spreadsheets/d/138bO8wfkjkVsRuBtcULuebSl0cgU1pL9fJxGIrBbuLk/export?format=tsv
        public static FileInfo DownloadFileFromURLToPath(string url, string path)
        {
            if (url.StartsWith(GoogleDriveDomain) || url.StartsWith(GoogleDriveDomain2))
                return DownloadGoogleDriveFileFromURLToPath(url, path);

            return DownloadFileFromURLToPath(url, path, null);
        }

        private static FileInfo DownloadFileFromURLToPath(string url, string path, WebClient webClient)
        {
            try
            {
                if (webClient == null)
                    using (webClient = new WebClient())
                    {
                        webClient.DownloadFile(url, path);
                        return new FileInfo(path);
                    }

                webClient.DownloadFile(url, path);
                return new FileInfo(path);
            }
            catch (WebException)
            {
                return null;
            }
        }

        // Downloading large files from Google Drive prompts a warning screen and
        // requires manual confirmation. Consider that case and try to confirm the download automatically
        // if warning prompt occurs
        private static FileInfo DownloadGoogleDriveFileFromURLToPath(string url, string path)
        {
            // You can comment the statement below if the provided url is guaranteed to be in the following format:
            // https://drive.google.com/uc?id=FILEID&export=download
            url = GetGoogleDriveDownloadLinkFromUrl(url);

            using CookieAwareWebClient webClient = new CookieAwareWebClient();

            FileInfo downloadedFile;

            // Sometimes Drive returns an NID cookie instead of a download_warning cookie at first attempt,
            // but works in the second attempt
            for (int i = 0; i < 2; i++)
            {
                downloadedFile = DownloadFileFromURLToPath(url, path, webClient);
                if (downloadedFile == null) return null;

                // Confirmation page is around 50KB, shouldn't be larger than 60KB
                if (downloadedFile.Length > 60000) return downloadedFile;

                // Downloaded file might be the confirmation page, check it
                string content;

                using (StreamReader reader = downloadedFile.OpenText())
                {
                    // Confirmation page starts with <!DOCTYPE html>, which can be preceeded by a newline
                    char[] header = new char[20];
                    int readCount = reader.ReadBlock(header, 0, 20);
                    if (readCount < 20 || !(new string(header).Contains("<!DOCTYPE html>"))) return downloadedFile;

                    content = reader.ReadToEnd();
                }

                int linkIndex = content.LastIndexOf("href=\"/uc?", StringComparison.Ordinal);
                if (linkIndex < 0) return downloadedFile;

                linkIndex += 6;
                int linkEnd = content.IndexOf('"', linkIndex);
                if (linkEnd < 0) return downloadedFile;

                url = "https://drive.google.com"
                    + content.Substring(linkIndex, linkEnd - linkIndex).Replace("&amp;", "&");
            }

            downloadedFile = DownloadFileFromURLToPath(url, path, webClient);

            return downloadedFile;
        }

        // Handles 3 kinds of links (they can be preceded by https://):
        // - drive.google.com/open?id=FILEID
        // - drive.google.com/file/d/FILEID/view?usp=sharing
        // - drive.google.com/uc?id=FILEID&export=download
        public static string GetGoogleDriveDownloadLinkFromUrl(string url)
        {
            int index = url.IndexOf("id=", StringComparison.Ordinal);
            int closingIndex;

            if (index > 0)
            {
                index += 3;
                closingIndex = url.IndexOf('&', index);
                if (closingIndex < 0) closingIndex = url.Length;
            }
            else
            {
                index = url.IndexOf("file/d/", StringComparison.Ordinal);

                if (index < 0) // url is not in any of the supported forms
                    return string.Empty;

                index += 7;

                closingIndex = url.IndexOf('/', index);

                if (closingIndex >= 0)
                    return
                        $"https://drive.google.com/uc?id={url.Substring(index, closingIndex - index)}&export=download";

                closingIndex = url.IndexOf('?', index);
                if (closingIndex < 0) closingIndex = url.Length;
            }

            return $"https://drive.google.com/uc?id={url.Substring(index, closingIndex - index)}&export=download";
        }
    }

    // Web client used for Google Drive
    public class CookieAwareWebClient : WebClient
    {
        private class CookieContainer
        {
            private readonly Dictionary<string, string> cookies;

            public string this[Uri url]
            {
                get => cookies.TryGetValue(url.Host, out string cookie) ? cookie : null;
                set => cookies[url.Host] = value;
            }

            public CookieContainer() => cookies = new Dictionary<string, string>();
        }

        private readonly CookieContainer cookies;

        public CookieAwareWebClient() => cookies = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (!(request is HttpWebRequest webRequest)) return request;
            string cookie = cookies[address];
            if (cookie != null) webRequest.Headers.Set("cookie", cookie);

            return webRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);

            string[] values = response.Headers.GetValues("Set-Cookie");

            if (values == null || values.Length <= 0) return response;
            string cookie = "";
            foreach (string c in values) cookie += c;

            cookies[response.ResponseUri] = cookie;

            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);

            string[] values = response?.Headers.GetValues("Set-Cookie");

            if (values == null || values.Length <= 0) return response;
            string cookie = "";
            foreach (string c in values) cookie += c;

            cookies[response.ResponseUri] = cookie;

            return response;
        }
    }
}