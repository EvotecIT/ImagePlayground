using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    private class FakeHttpHandler : HttpMessageHandler {
        private readonly HttpResponseMessage _response;
        public FakeHttpHandler(HttpResponseMessage response) {
            _response = response;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            return Task.FromResult(_response);
        }
    }

    [Fact]
    public async Task Test_GetStringWithProperEncoding_HeaderCharset() {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string text = "zażółć";
        var bytes = Encoding.GetEncoding("iso-8859-2").GetBytes(text);
        var response = new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new ByteArrayContent(bytes)
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain") { CharSet = "iso-8859-2" };
        using var client = new HttpClient(new FakeHttpHandler(response));
        string result = await Helpers.GetStringWithProperEncodingAsync(client, "http://test");
        Assert.Equal(text, result);
    }

    [Fact]
    public async Task Test_GetStringWithProperEncoding_BomUtf8() {
        byte[] bytes = Encoding.UTF8.GetPreamble();
        bytes = bytes.Concat(Encoding.UTF8.GetBytes("hello")).ToArray();
        var response = new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new ByteArrayContent(bytes)
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        using var client = new HttpClient(new FakeHttpHandler(response));
        string result = await Helpers.GetStringWithProperEncodingAsync(client, "http://test");
        Assert.Equal("hello", result);
    }

    [Fact]
    public async Task Test_GetStringWithProperEncoding_BomUtf16() {
        byte[] prefix = Encoding.Unicode.GetPreamble();
        byte[] bytes = prefix.Concat(Encoding.Unicode.GetBytes("hi")).ToArray();
        var response = new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new ByteArrayContent(bytes)
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        using var client = new HttpClient(new FakeHttpHandler(response));
        string result = await Helpers.GetStringWithProperEncodingAsync(client, "http://test");
        Assert.Equal("hi", result);
    }

    [Fact]
    public async Task Test_GetStringWithProperEncoding_MetaTag() {
        string html = "<meta charset=\"utf-8\">Hello";
        byte[] bytes = Encoding.ASCII.GetBytes(html);
        var response = new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new ByteArrayContent(bytes)
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        using var client = new HttpClient(new FakeHttpHandler(response));
        string result = await Helpers.GetStringWithProperEncodingAsync(client, "http://test");
        Assert.Equal("<meta charset=\"utf-8\">Hello", result);
    }
}
