using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Classifieds.Domain.Uploadcare
{
    public class Uploadcare
    {
        private HttpWebRequest request;
        private HttpWebResponse response;
        private string baseUrl;
        private StreamReader streamReader;

        public Uploadcare()
        {
            baseUrl = "https://api.uploadcare.com/files/";

        }
        /// <summary>
        /// Delete a single file from storage
	    /// url: https://api.uploadcare.com/files/ + uuid + /
	    /// type: DELETE
        /// JSON response contains the following
	    ///     -datetime_removed
        ///     -datetime_stored
        ///     -datetime_uploaded
        ///     -image_info: 
	    /// 	{
	    ///     		color_mode
        ///             datetime_original
	    ///             dpi": [w,h]
	    ///             format
        ///             geo_location
	    ///             height
        ///             orientation
	    ///             width
        ///
        ///     }
        /// -is_image
        /// -is_ready
        /// -mime_type
        /// -original_file_url
        /// -original_filename
        /// -size
        /// -source
        /// -url
        /// -uuid

        /// </summary>
        /// <param name="uuid">file uuid</param>
        /// <returns>JSON string</returns>
        public string DeleteFile(string uuid)
        {
            string httpContent = "";
            string targetUrl = baseUrl + uuid + "/";

            try
            {
                request = (HttpWebRequest)WebRequest.Create(targetUrl);
                setHeaders();
                request.Method = "DELETE";
                response = (HttpWebResponse)request.GetResponse();

                streamReader = new StreamReader(response.GetResponseStream());

                httpContent = streamReader.ReadToEnd();
                httpContent = httpContent.Substring(0, httpContent.Length - 1);
                httpContent += ",\"status\":200}";
                
                response.Close();
            }
            catch(Exception ex)
            {
                httpContent = "{\"status\":400}";
                System.Console.Write(ex.StackTrace);
                
            }
  
            return httpContent;
        }
        /// <summary>
        /// Delete a group of images.The input is a list of uuids.
	    /// Response from uploadcare takes the following format
	    /// -problems: { }
	    /// -result: [
	    ///  		{
	    ///   			datetime_removed
	    ///   			datetime_stored
	    ///   			datetime_uploaded
	    ///   			image_info: {
	    ///   				color_mode
	    ///   				datetime_original
	    ///   				dpi: [w, h]
        ///                 format
	    ///                 geo_location
        ///                 height
	    ///                 orientation
        ///                 width
	    ///             }
        ///     is_image
        ///     is_ready
        ///     mime_type
        ///     original_file_url
        ///     original_filename
        ///     size
        ///     source
        ///     url
        ///     uuid
        ///   }
        /// ]
        /// -status
        /// 
        /// When theres an error the response is as follows
        /// "problems": {
        ///    "uuid1": "Invalid",
        ///    "uuid2": "Invalid"
        ///     },
        /// "result": [],
        /// "status": "ok"
        /// </summary>
        /// <param name="uuidGroup">List uuid to delete</param>
        /// <returns>json string response</returns>
        public string DeleteBatch(List<string> uuidGroup)
        {
            string httpContent = "";
            string targetUrl = baseUrl + "storage/";
            string jsonString;
            try
            {
                jsonString = "[";

                foreach(var uuid in uuidGroup)
                {
                    jsonString += "\"" + uuid + "\"" + ",";
                }

                jsonString = jsonString.Substring(0, jsonString.Length - 1);
                jsonString += "]";


                request = (HttpWebRequest)WebRequest.Create(targetUrl);
                setHeaders();
                request.Method = "DELETE";

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(Encoding.ASCII.GetBytes(jsonString),0,jsonString.Length);
                dataStream.Close();

                response = (HttpWebResponse)request.GetResponse();

                streamReader = new StreamReader(response.GetResponseStream());

                httpContent = streamReader.ReadToEnd();

                response.Close();
            }
            catch (Exception ex)
            {

            }

            return httpContent;
        }
        private void setHeaders()
        {
            request.Headers.Set(HttpRequestHeader.Accept,
                "application/vnd.uploadcare-v0.5+json");
            request.Headers.Set(HttpRequestHeader.Authorization,
                "Uploadcare.Simple 1f4ebb5d14270931faf4:dac2dd4b65eea7a48b68");//classiifieds project
            request.Headers.Set(HttpRequestHeader.ContentType, "application/json");
        }
    }
}
