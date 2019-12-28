// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonContent.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class JsonContent : HttpContent
    {
        #region Fields
        private readonly object _value;
        #endregion

        #region Constructors
        public JsonContent(object value)
        {
            _value = value;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
        #endregion

        #region Methods
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var json = JsonConvert.SerializeObject(_value);

            using (var jw = new JsonTextWriter(new StreamWriter(stream)))
            {
                await jw.WriteRawAsync(json);
                await jw.FlushAsync();
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
        #endregion
    }
}
