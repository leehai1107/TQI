using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PasswordEncrypt;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TQI.NBTeam.Commons;
using TQI.NBTeam.Models;

namespace TQI.NBTeam.Handlers;

public class FacebookHandler
{
    public enum StatusCookie
    {
        Live,
        Die,
        Error,
        Checkpoint282,
        Checkpoint956,
        CheckpointCaptcha,
        Checkpoint681,
        Noveri,
        WrongPassword,
        Error2FA
    }

    public enum FriendStatus
    {
        OutGoing,
        AreFriend,
        Error,
        Checkpoint,
        UidCheckpoint,
        SendError,
        Unfriend
    }

    public enum StatusShareBM
    {
        Success,
        Failed,
        Checkpoint282,
        Checkpoint956,
        Error
    }

    public enum StatusCreateBM
    {
        Success,
        Fail,
        Error,
        Checkpoint,
        Block,
        ReLogin
    }

    public enum StatusJoinBM
    {
        Already,
        Success,
        Error,
        Checkpoint282,
        Checkpoint956,
        EmptyData,
        Block
    }

    public AccountDto _account;

    private readonly string _userAgent;

    private readonly string _proxy;

    private HttpClient client;

    private CookieContainer cookieContainer;

    public FacebookHandler(AccountDto account, string useragent, string proxy)
    {
        _account = account;
        _userAgent = useragent;
        _proxy = proxy;
    }

    private void InitializeHttpClient(string proxy, int typeLogin = 0)
    {
        if (typeLogin == 0)
        {
            string cookie = _account.Cookie;
            string userAgent = _userAgent;
            (client, cookieContainer) = HttpClientCommon.CreateHttpClient(cookie, proxy, userAgent);
        }
        else
        {
            string userAgent = _userAgent;
            (client, cookieContainer) = HttpClientCommon.CreateHttpClient("", proxy, userAgent);
        }
    }

    public void LoginCookie()
    {
        (client, cookieContainer) = HttpClientCommon.CreateHttpClient(_account.Cookie, useragent: _userAgent, proxyStr: _proxy);
    }

    public async Task<StatusCookie> LoginFacebook(int typeLogin = 0)
    {
        try
        {
            _account.DTSGToken = string.Empty;
            _account.LSDToken = string.Empty;
            InitializeHttpClient(_proxy, typeLogin);
            return typeLogin switch
            {
                0 => await CheckCookieStatus(),
                1 => await LoginFacebookUidPass(),
                2 => await LoginAndroid(),
                _ => throw new NotImplementedException(),
            };
        }
        catch
        {
            return StatusCookie.Error;
        }
    }

    public async Task<StatusCookie> CheckCookieStatus()
    {
        HttpResponseMessage response = await client.GetWithRetry("https://www.facebook.com/", _account);
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        string dtsgToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
        string lsdToken = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
        _ = Regex.Match(responseStr, "jazoest\":(\\d+)").Groups[1].Value;
        if (response.RequestMessage.RequestUri.AbsoluteUri.Contains("pft_user_cookie_choice"))
        {
            string experience_id = Regex.Match(responseStr, "experience_id\":\"(.*?)\"").Groups[1].Value.ToString();
            response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "fb_dtsg", dtsgToken },
                { "jazoest", "25425" },
                { "lsd", "Ag2WcUbeWHTbItjPdnEBQp" },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "useSaharaCometConsentPostPromptOutcomeServerMutation" },
                {
                    "variables",
                    "{\"input\":{\"client_mutation_id\":\"2\",\"actor_id\":\"" + _account.Uid + "\",\"device_id\":null,\"experience_id\":\"" + experience_id + "\",\"extra_params_json\":\"{\\\"__aectx__\\\":\\\"{\\\\\\\"id\\\\\\\":\\\\\\\"" + experience_id + "\\\\\\\",\\\\\\\"flows\\\\\\\":[{\\\\\\\"id\\\\\\\":\\\\\\\"user_cookie_choice_v2\\\\\\\",\\\\\\\"prompts\\\\\\\":[{\\\\\\\"id\\\\\\\":\\\\\\\"user_cookie_choice_granular_control\\\\\\\"}]}]}\\\"}\",\"flow\":\"USER_COOKIE_CHOICE_V2\",\"inputs_json\":\"{\\\"other_company_trackers_on_foa\\\":\\\"\\\",\\\"fb_trackers_on_other_companies\\\":\\\"\\\",\\\"advertising\\\":\\\"\\\",\\\"analytics\\\":\\\"\\\",\\\"content_and_media\\\":\\\"\\\",\\\"productivity\\\":\\\"\\\",\\\"ADOBE_MARKETO\\\":\\\"\\\",\\\"GOOGLE_ADS\\\":\\\"\\\",\\\"KOCHAVA\\\":\\\"\\\",\\\"LINKEDIN_MARKETING\\\":\\\"\\\",\\\"X_ADS\\\":\\\"\\\",\\\"GOOGLE_ANALYTICS\\\":\\\"\\\",\\\"MEDALLIA\\\":\\\"\\\",\\\"AMAZON_AWS_CONTENT\\\":\\\"\\\",\\\"BLINGS_IO\\\":\\\"\\\",\\\"CLOUDFRONT\\\":\\\"\\\",\\\"GIPHY\\\":\\\"\\\",\\\"GOOGLE_MEDIA\\\":\\\"\\\",\\\"NEW_YORK_TIMES\\\":\\\"\\\",\\\"SOUNDCLOUD\\\":\\\"\\\",\\\"SPOTIFY\\\":\\\"\\\",\\\"SPREAKER\\\":\\\"\\\",\\\"TED\\\":\\\"\\\",\\\"TENOR\\\":\\\"\\\",\\\"TIKTOK\\\":\\\"\\\",\\\"VIMEO\\\":\\\"\\\",\\\"X\\\":\\\"\\\",\\\"YOUTUBE\\\":\\\"\\\",\\\"CHILI_PIPER\\\":\\\"\\\",\\\"GOOGLE\\\":\\\"\\\",\\\"JIO\\\":\\\"\\\",\\\"MAPBOX\\\":\\\"\\\",\\\"MICROSOFT\\\":\\\"\\\",\\\"card_index_0_learnt_more\\\":\\\"\\\",\\\"card_index_1_learnt_more\\\":\\\"\\\",\\\"card_index_2_learnt_more\\\":\\\"\\\",\\\"card_index_3_learnt_more\\\":\\\"\\\"}\",\"outcome\":\"APPROVED\",\"outcome_data_json\":\"{}\",\"prompt\":\"USER_COOKIE_CHOICE_GRANULAR_CONTROL\",\"runtime\":\"SAHARA\",\"source\":\"pft_user_cookie_choice\",\"surface\":\"FACEBOOK_COMET\"}}"
                },
                { "locale", "en_GB" },
                { "server_timestamps", "true" },
                { "doc_id", "8906652576046836" }
            }), client: client, url: "https://business.facebook.com/api/graphql/", account: _account);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCookie.Error;
            }
            MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
            contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
            response = await client.GetWithRetry("https://www.facebook.com/me", _account);
            MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
            contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
        }
        else if (response.RequestMessage.RequestUri.AbsolutePath.Contains("is_new_user_blocking_flow"))
        {
            string experience_id2 = Regex.Match(responseStr, "experience_id\":\"(.*?)\"").Groups[1].Value.ToString();
            string gcl_experience_id = Regex.Match(responseStr, "gcl_experience_id\\\\\":\\\\\"(.*?)\\\\\"").Groups[1].Value.ToString();
            string pft_session_key = Regex.Match(responseStr, "pft_session_key\\\\\":\\\\\"(.*?)\\\\\"").Groups[1].Value.ToString();
            response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "fb_dtsg", dtsgToken },
                { "jazoest", "25425" },
                { "lsd", "Ag2WcUbeWHTbItjPdnEBQp" },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "useConsentLoggingPlatformGQLEndpointMutation" },
                {
                    "variables",
                    "{\"input\":{\"client_mutation_id\":\"5\",\"actor_id\":\"" + _account.Uid + "\",\"event\":\"consent_interactions_toggle_on\",\"event_data\":\"{\\\"name\\\":\\\"data_shared_3pd_toggle\\\",\\\"value\\\":\\\"true\\\"}\",\"table\":\"consent_interactions\",\"top_level_columns\":\"{\\\"config_enum\\\":\\\"pipa_main\\\",\\\"device_id\\\":null,\\\"experience_id\\\":\\\"" + experience_id2 + "\\\",\\\"extra_params\\\":\\\"{\\\\\\\"pft_surface\\\\\\\":\\\\\\\"facebook_comet\\\\\\\",\\\\\\\"is_new_user_blocking_flow\\\\\\\":\\\\\\\"true\\\\\\\",\\\\\\\"gcl_experience_id\\\\\\\":\\\\\\\"" + gcl_experience_id + "\\\\\\\",\\\\\\\"pft_session_key\\\\\\\":\\\\\\\"" + pft_session_key + "\\\\\\\"}\\\",\\\"flow_name\\\":\\\"pipa\\\",\\\"source\\\":\\\"pipa_blocking_flow\\\",\\\"surface\\\":\\\"facebook_comet\\\",\\\"runtime\\\":\\\"comet\\\"}\"}}"
                },
                { "locale", "en_GB" },
                { "server_timestamps", "true" },
                { "doc_id", "7733733796695507" }
            }), client: client, url: "https://business.facebook.com/api/graphql/", account: _account);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCookie.Error;
            }
            MediaTypeHeaderValue contentType5 = response.Content.Headers.ContentType;
            contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
            response = await client.GetWithRetry("https://www.facebook.com/me", _account);
            MediaTypeHeaderValue contentType6 = response.Content.Headers.ContentType;
            contentType2 = ((contentType6 != null) ? contentType6.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
        }
        else if (response.RequestMessage.RequestUri.AbsolutePath.Contains("601051028565049"))
        {
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "__req", "5" },
                { "__hs", "20025.HYP:comet_pkg.2.1..2.1" },
                { "dpr", "1" },
                { "__ccg", "EXCELLENT" },
                { "__rev", "1017759046" },
                { "__s", "mwvlhs:4ve1ex:dagn50" },
                { "__hsi", "7431233835583946046" },
                { "__dyn", "7xeUmwlEnwn8K2Wmh0no6u5U4e0yoW3q32360CEbo19oe8hw2nVE4W099w8G1Dz81s8hwnU2lwv89k2C1Fwc60D8vwRwlE-U2zxe2GewbS361qw8Xwn82Lx-0lK3qazo720Bo2ZwrU6C0hq1Iwqo35wvodo7u2-2K0UE" },
                { "__csr", "iNCvuV4h4DGUHGXUSeZ4DjrwzyGyJbzpUO2G59E4-exaU4vwbu4o1ao4i0nS2u0gi2W1Jw3wE4m0EU1eU0zK00oeC034S00XgE" },
                { "__comet_req", "15" },
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25401" },
                { "lsd", "_SUj0S8K_WcCtiwQFwev4Y" },
                { "__spin_r", "1017759046" },
                { "__spin_b", "trunk" },
                { "__spin_t", "1730218957" },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "FBScrapingWarningMutation" },
                { "variables", "{}" },
                { "server_timestamps", "true" },
                { "doc_id", "6339492849481770" }
            };
            await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        }
        response = await client.GetWithRetry("https://www.facebook.com/me", _account);
        MediaTypeHeaderValue contentType7 = response.Content.Headers.ContentType;
        contentType2 = ((contentType7 != null) ? contentType7.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        responseStr = await response.Content.ReadAsStringAsync();
        string userId = Regex.Match(responseStr, "userid\":(\\d+)").Groups[1].Value.Trim();
        string userId2 = Regex.Match(responseStr, "\"USER_ID\":\"(.*?)\"").Groups[1].Value.Trim();
        if (string.IsNullOrEmpty(dtsgToken))
        {
            dtsgToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
            lsdToken = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
            _ = Regex.Match(responseStr, "jazoest=(\\d+)").Groups[1].Value;
        }
        if (response.RequestMessage.RequestUri.AbsolutePath.Contains("checkpoint/1501092823525282"))
        {
            return StatusCookie.Checkpoint282;
        }
        if (response.RequestMessage.RequestUri.AbsolutePath.Contains("828281030927956"))
        {
            return StatusCookie.Checkpoint956;
        }
        if ((!string.IsNullOrEmpty(userId) || !string.IsNullOrEmpty(userId2)) && (Regex.Match(responseStr, "\"USER_ID\":\"(.*?)\"").Groups[1].Value.Trim() == _account.Uid.Trim() || Regex.Match(responseStr, "userid\":(\\d+)").Groups[1].Value.Trim() == _account.Uid.Trim()))
        {
            string cookies = await cookieContainer.CookieToString();
            _account.Cookie = cookies;
            _account.DTSGToken = dtsgToken;
            _account.LSDToken = lsdToken;
            return StatusCookie.Live;
        }
        return StatusCookie.Die;
    }

    private async Task<StatusCookie> LoginFacebookUidPass()
    {
        try
        {
            if (!string.IsNullOrEmpty(_account.Cookie))
            {
                string[] cookieJar = _account.Cookie.Split(';');
                string[] array = cookieJar;
                foreach (string cookie in array)
                {
                    try
                    {
                        string[] cookieItem = cookie.Split('=');
                        if (cookieItem.Length >= 2 && (cookieItem[0].Trim().Equals("datr") || cookieItem[0].Trim().Equals("xs")))
                        {
                            cookieContainer.Add(new Cookie(cookieItem[0].Trim(), cookieItem[1].Trim(), "/", ".facebook.com"));
                        }
                    }
                    catch
                    {
                    }
                }
            }
            HttpResponseMessage response = await client.GetAsync("https://www.facebook.com/");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCookie.Error;
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            if (((contentType != null) ? contentType.CharSet : null)?.Contains("\"utf-8\"") ?? false)
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            string publicKey = Regex.Match(responseStr, "\"publicKey\":\"(.*?)\"").Groups[1].Value;
            string keyId = Regex.Match(responseStr, "\"keyId\":(.*?)}").Groups[1].Value;
            string action = Regex.Match(responseStr, "action=\"(.*?)\"").Groups[1].Value.Replace("amp;", "");
            string datr = Regex.Match(responseStr, "\"_js_datr\",\"(.*?)\"").Groups[1].Value;
            string lsd = Regex.Match(responseStr, "LSD\"(.*?){\"token\":\"(.*?)\"").Groups[2].Value;
            string jazoest = (string.IsNullOrEmpty(Regex.Match(responseStr, "name=\"jazoest\" value=\"(\\d+)\"").Groups[1].Value) ? Regex.Match(responseStr, "jazoest\",\"value\":\"(.*?)\"").Groups[1].Value : Regex.Match(responseStr, "name=\"jazoest\" value=\"(\\d+)\"").Groups[1].Value);
            string originUri = "https://www.facebook.com" + action;
            if (string.IsNullOrEmpty(publicKey))
            {
                publicKey = "cda78c8ea177c03e8dd655225dca91e3b79eca4ec114f58832234c9b1d2a8578";
                keyId = "47";
            }
            string passwordEncrypt = FacebookEncryptHelper.GenerateEncPassword(_account.Password, publicKey, keyId, "5");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, originUri);
            if (!string.IsNullOrEmpty(datr))
            {
                cookieContainer.Add(new Cookie("datr", datr, "/", ".facebook.com"));
            }
            httpRequestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new KeyValuePair<string, string>[6]
            {
                new KeyValuePair<string, string>("jazoest", jazoest),
                new KeyValuePair<string, string>("lsd", lsd),
                new KeyValuePair<string, string>("email", _account.Uid),
                new KeyValuePair<string, string>("login_source", "comet_headerless_login"),
                new KeyValuePair<string, string>("next", ""),
                new KeyValuePair<string, string>("encpass", passwordEncrypt)
            });
            HttpResponseMessage responseLogin = await client.SendRequest(httpRequestMessage);
            if (!responseLogin.IsSuccessStatusCode)
            {
                return StatusCookie.Error;
            }
            if (responseLogin.RequestMessage.RequestUri.AbsoluteUri == "https://www.facebook.com/?lsrc=lb")
            {
                return StatusCookie.Live;
            }
            if (responseLogin.RequestMessage.RequestUri.AbsolutePath == "/two_step_verification/authentication/")
            {
                MediaTypeHeaderValue contentType2 = responseLogin.Content.Headers.ContentType;
                if (((contentType2 != null) ? contentType2.CharSet : null)?.Contains("\"utf-8\"") ?? false)
                {
                    responseLogin.Content.Headers.ContentType.CharSet = "utf-8";
                }
                await responseLogin.Content.ReadAsStringAsync();
                return StatusCookie.CheckpointCaptcha;
            }
            if (responseLogin.RequestMessage.RequestUri.AbsolutePath == "681")
            {
                return StatusCookie.Checkpoint681;
            }
            MediaTypeHeaderValue contentType3 = responseLogin.Content.Headers.ContentType;
            if (((contentType3 != null) ? contentType3.CharSet : null)?.Contains("\"utf-8\"") ?? false)
            {
                responseLogin.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string contentLogin = await responseLogin.Content.ReadAsStringAsync();
            await cookieContainer.CookieToString();
            if (responseLogin.RequestMessage.RequestUri.AbsoluteUri.Contains("checkpoint/1501092823525282") || responseLogin.RequestMessage.RequestUri.AbsoluteUri.Contains("828281030927956"))
            {
                return StatusCookie.Checkpoint282;
            }
            if (responseLogin.RequestMessage.RequestUri.AbsoluteUri.Contains("checkpoint/601051028565049"))
            {
                _account.DTSGToken = Regex.Match(contentLogin, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                Dictionary<string, string> payload = new Dictionary<string, string>
                {
                    { "av", _account.Uid },
                    { "__user", _account.Uid },
                    { "__a", "1" },
                    { "__req", "5" },
                    { "__hs", "20025.HYP:comet_pkg.2.1..2.1" },
                    { "dpr", "1" },
                    { "__ccg", "EXCELLENT" },
                    { "__rev", "1017759046" },
                    { "__s", "mwvlhs:4ve1ex:dagn50" },
                    { "__hsi", "7431233835583946046" },
                    { "__dyn", "7xeUmwlEnwn8K2Wmh0no6u5U4e0yoW3q32360CEbo19oe8hw2nVE4W099w8G1Dz81s8hwnU2lwv89k2C1Fwc60D8vwRwlE-U2zxe2GewbS361qw8Xwn82Lx-0lK3qazo720Bo2ZwrU6C0hq1Iwqo35wvodo7u2-2K0UE" },
                    { "__csr", "iNCvuV4h4DGUHGXUSeZ4DjrwzyGyJbzpUO2G59E4-exaU4vwbu4o1ao4i0nS2u0gi2W1Jw3wE4m0EU1eU0zK00oeC034S00XgE" },
                    { "__comet_req", "15" },
                    { "fb_dtsg", _account.DTSGToken },
                    { "jazoest", "25401" },
                    { "lsd", "_SUj0S8K_WcCtiwQFwev4Y" },
                    { "__spin_r", "1017759046" },
                    { "__spin_b", "trunk" },
                    { "__spin_t", "1730218957" },
                    { "fb_api_caller_class", "RelayModern" },
                    { "fb_api_req_friendly_name", "FBScrapingWarningMutation" },
                    { "variables", "{}" },
                    { "server_timestamps", "true" },
                    { "doc_id", "6339492849481770" }
                };
                response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
                MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
                if (((contentType4 != null) ? contentType4.CharSet : null)?.Contains("\"utf-8\"") ?? false)
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                await response.Content.ReadAsStringAsync();
                responseLogin = await client.GetAsync("https://www.facebook.com/");
                MediaTypeHeaderValue contentType5 = responseLogin.Content.Headers.ContentType;
                if (((contentType5 != null) ? contentType5.CharSet : null)?.Contains("\"utf-8\"") ?? false)
                {
                    responseLogin.Content.Headers.ContentType.CharSet = "utf-8";
                }
                contentLogin = await responseLogin.Content.ReadAsStringAsync();
            }
            if (responseLogin.RequestMessage.RequestUri.OriginalString.Contains("welcome"))
            {
                return StatusCookie.Live;
            }
            if (!(responseLogin.RequestMessage.RequestUri.AbsoluteUri != "https://www.facebook.com/?sk=welcome&lsrc=lb") || !(responseLogin.RequestMessage.RequestUri.AbsoluteUri != "https://www.facebook.com/"))
            {
                return await CheckCookieStatus();
            }
            string encryptContext = Regex.Match(contentLogin, "encrypted_context=(.*?)&").Groups[1].Value;
            if (string.IsNullOrEmpty(encryptContext))
            {
                return StatusCookie.WrongPassword;
            }
            if (responseLogin.RequestMessage.RequestUri.AbsolutePath == "/two_step_verification/two_factor/")
            {
                HttpResponseMessage responseVerification = await client.GetAsync("https://www.facebook.com/checkpoint");
                if (!responseVerification.IsSuccessStatusCode)
                {
                    return StatusCookie.Error;
                }
                MediaTypeHeaderValue contentType6 = responseVerification.Content.Headers.ContentType;
                if (((contentType6 != null) ? contentType6.CharSet : null)?.Contains("\"utf-8\"") ?? false)
                {
                    responseVerification.Content.Headers.ContentType.CharSet = "utf-8";
                }
                responseStr = await responseVerification.Content.ReadAsStringAsync();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(responseStr);
                string fbdtsg = document.DocumentNode.SelectNodes("//input[@name='fb_dtsg']").FirstOrDefault().Attributes["value"].Value;
                _ = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                jazoest = document.DocumentNode.SelectNodes("//input[@name='jazoest']").FirstOrDefault().Attributes["value"].Value;
                string nh = document.DocumentNode.SelectNodes("//input[@name='nh']").FirstOrDefault().Attributes["value"].Value;
                string buttonName = document.DocumentNode.SelectSingleNode("//button[@id='checkpointSubmitButton']").Attributes["value"].Value;
                string code2Fa = Common.GetCode(_account.Key2FA);
                Dictionary<string, string> payload2 = new Dictionary<string, string>
                {
                    { "jazoest", jazoest },
                    { "fb_dtsg", fbdtsg },
                    { "nh", nh },
                    { "approvals_code", code2Fa },
                    { "submit[Continue]", buttonName }
                };
                HttpResponseMessage responseSubmitCode = await client.PostAsync("https://www.facebook.com/checkpoint/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload2));
                MediaTypeHeaderValue contentType7 = responseSubmitCode.Content.Headers.ContentType;
                if (((contentType7 != null) ? contentType7.CharSet : null)?.Contains("\"utf-8\"") ?? false)
                {
                    responseSubmitCode.Content.Headers.ContentType.CharSet = "utf-8";
                }
                string responseSubmitCodeStr = await responseSubmitCode.Content.ReadAsStringAsync();
                if (responseSubmitCode.RequestMessage.RequestUri.AbsolutePath == "/checkpoint/")
                {
                    document = new HtmlDocument();
                    document.LoadHtml(responseSubmitCodeStr);
                    HtmlNodeCollection tbApproval = document.DocumentNode.SelectNodes("//input[@id='approvals_code']");
                    if (tbApproval != null && tbApproval.Count > 0)
                    {
                        return StatusCookie.Error2FA;
                    }
                    HtmlNodeCollection formData = document.DocumentNode.SelectNodes("//form[@class='checkpoint']//input");
                    do
                    {
                        payload2 = new Dictionary<string, string>();
                        foreach (HtmlNode item in formData)
                        {
                            if (string.IsNullOrEmpty(item.Attributes["value"]?.Value))
                            {
                                payload2.Add(item.Attributes["name"].Value, item.InnerText);
                            }
                            else if (!payload2.ContainsKey(item.Attributes["name"].Value))
                            {
                                payload2.Add(item.Attributes["name"].Value, item.Attributes["value"].Value);
                            }
                        }
                        response = await client.PostAsync("https://www.facebook.com/checkpoint/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload2));
                        MediaTypeHeaderValue contentType8 = response.Content.Headers.ContentType;
                        if (((contentType8 != null) ? contentType8.CharSet : null)?.Contains("\"utf-8\"") ?? false)
                        {
                            response.Content.Headers.ContentType.CharSet = "utf-8";
                        }
                        responseStr = await response.Content.ReadAsStringAsync();
                        document = new HtmlDocument();
                        document.LoadHtml(responseStr);
                        formData = document.DocumentNode.SelectNodes("//form[@class='checkpoint']//input");
                    }
                    while (formData != null && formData.Count > 0);
                }
                return await CheckCookieStatus();
            }
        }
        catch (Exception)
        {
            return StatusCookie.Error;
        }
        return StatusCookie.Die;
    }

    private async Task<StatusCookie> LoginAndroid()
    {
        try
        {
            string datalogin = "{\"locale\":\"en_US\",\"format\":\"json\",\"email\":\"" + _account.Uid + "\",\"password\":\"" + _account.Password + "\",\"access_token\":\"350685531728|62f8ce9f74b12f84c123cc23437a4a32\",\"generate_session_cookies\":true}";
            StringContent content = new StringContent(datalogin, (Encoding)null, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://graph.facebook.com/auth/login", (HttpContent)(object)content);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            if (((contentType != null) ? contentType.CharSet : null)?.Contains("\"utf-8\"") ?? false)
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("checkpoint/1501092823525282"))
            {
                return StatusCookie.Checkpoint282;
            }
            if (responseStr.Contains("code\": 613") || responseStr.Contains("Invalid username or password") || responseStr.Contains("Invalid username or email address") || responseStr.Contains("The action attempted has been deemed abusive or is otherwise disallowed") || responseStr.Contains("User must verify their account on www.facebook.com"))
            {
                return StatusCookie.Error;
            }
            if (responseStr.Contains("code\": 406"))
            {
                return StatusCookie.Error2FA;
            }
            JObject responseObject = JObject.Parse(responseStr);
            JArray cookieItemArr = responseObject["session_cookies"].ToObject<JArray>();
            foreach (JToken item in cookieItemArr)
            {
                cookieContainer.Add(new Cookie(item["name"].ToString().Trim(), item["value"].ToString().Trim(), "/", ".facebook.com"));
            }
            return StatusCookie.Live;
        }
        catch
        {
            return StatusCookie.Error;
        }
    }

    public async Task<(string, string)> GetTokenAsync()
    {
        string token = string.Empty;
        string dtsgToken = string.Empty;
        _ = string.Empty;
        try
        {
            HttpResponseMessage response = await client.GetWithRetry("https://business.facebook.com/billing_hub/payment_activity?asset_id=", _account);
            if (response.IsSuccessStatusCode)
            {
                MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
                string contentType2 = ((contentType != null) ? contentType.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                string responseStr = await response.Content.ReadAsStringAsync();
                token = Regex.Match(responseStr, "(EAAG(.*?)ZDZD)").Value.Replace("\"", "").Replace("\\", "");
                if (!string.IsNullOrEmpty(token))
                {
                    _account.Token = token;
                }
                else
                {
                    string redirectUrl = Regex.Match(responseStr, "redirectUri\":\"(.*?)\"}").Groups[1].Value.Replace("\\", "");
                    if (!string.IsNullOrEmpty(redirectUrl))
                    {
                        responseStr = await (await client.GetWithRetry(redirectUrl, _account)).Content.ReadAsStringAsync();
                        token = Regex.Match(responseStr, "(EAAG(.*?)ZDZD)").Value.Replace("\"", "").Replace("\\", "");
                        if (!string.IsNullOrEmpty(token))
                        {
                            _account.Token = token;
                        }
                        else
                        {
                            Dictionary<string, string> payload = new Dictionary<string, string>
                            {
                                { "av", _account.Uid },
                                { "__a", "1" },
                                { "dpr", "1" },
                                { "fb_dtsg", dtsgToken },
                                { "jazoest", "25668" },
                                { "lsd", "tbC0bJOdxzlMQiOsF3no8-" },
                                { "doc_id", "6494107973937368" },
                                {
                                    "variables",
                                    "{\"input\":{\"client_mutation_id\":\"4\",\"actor_id\":\"" + _account.Uid + "\",\"config_enum\":\"GDP_CONFIRM\",\"device_id\":null,\"experience_id\":\"ea0ce607-ab1d-4358-bdff-2dc18790cc5f\",\"extra_params_json\":\"{\\\"app_id\\\":\\\"436761779744620\\\",\\\"kid_directed_site\\\":\\\"false\\\",\\\"logger_id\\\":\\\"\\\\\\\"454a99cc-0d5a-4ed5-916b-987047ece6ce\\\\\\\"\\\",\\\"next\\\":\\\"\\\\\\\"confirm\\\\\\\"\\\",\\\"redirect_uri\\\":\\\"\\\\\\\"https:\\\\\\\\\\\\/\\\\\\\\\\\\/www.facebook.com\\\\\\\\\\\\/connect\\\\\\\\\\\\/login_success.html\\\\\\\"\\\",\\\"response_type\\\":\\\"\\\\\\\"token\\\\\\\"\\\",\\\"return_scopes\\\":\\\"false\\\",\\\"scope\\\":\\\"[\\\\\\\"user_subscriptions\\\\\\\",\\\\\\\"user_videos\\\\\\\",\\\\\\\"user_website\\\\\\\",\\\\\\\"user_work_history\\\\\\\",\\\\\\\"friends_about_me\\\\\\\",\\\\\\\"friends_actions.books\\\\\\\",\\\\\\\"friends_actions.music\\\\\\\",\\\\\\\"friends_actions.news\\\\\\\",\\\\\\\"friends_actions.video\\\\\\\",\\\\\\\"friends_activities\\\\\\\",\\\\\\\"friends_birthday\\\\\\\",\\\\\\\"friends_education_history\\\\\\\",\\\\\\\"friends_events\\\\\\\",\\\\\\\"friends_games_activity\\\\\\\",\\\\\\\"friends_groups\\\\\\\",\\\\\\\"friends_hometown\\\\\\\",\\\\\\\"friends_interests\\\\\\\",\\\\\\\"friends_likes\\\\\\\",\\\\\\\"friends_location\\\\\\\",\\\\\\\"friends_notes\\\\\\\",\\\\\\\"friends_photos\\\\\\\",\\\\\\\"friends_questions\\\\\\\",\\\\\\\"friends_relationship_details\\\\\\\",\\\\\\\"friends_relationships\\\\\\\",\\\\\\\"friends_religion_politics\\\\\\\",\\\\\\\"friends_status\\\\\\\",\\\\\\\"friends_subscriptions\\\\\\\",\\\\\\\"friends_videos\\\\\\\",\\\\\\\"friends_website\\\\\\\",\\\\\\\"friends_work_history\\\\\\\",\\\\\\\"ads_management\\\\\\\",\\\\\\\"create_event\\\\\\\",\\\\\\\"create_note\\\\\\\",\\\\\\\"export_stream\\\\\\\",\\\\\\\"friends_online_presence\\\\\\\",\\\\\\\"manage_friendlists\\\\\\\",\\\\\\\"manage_notifications\\\\\\\",\\\\\\\"manage_pages\\\\\\\",\\\\\\\"photo_upload\\\\\\\",\\\\\\\"publish_stream\\\\\\\",\\\\\\\"read_friendlists\\\\\\\",\\\\\\\"read_insights\\\\\\\",\\\\\\\"read_mailbox\\\\\\\",\\\\\\\"read_page_mailboxes\\\\\\\",\\\\\\\"read_requests\\\\\\\",\\\\\\\"read_stream\\\\\\\",\\\\\\\"rsvp_event\\\\\\\",\\\\\\\"share_item\\\\\\\",\\\\\\\"sms\\\\\\\",\\\\\\\"status_update\\\\\\\",\\\\\\\"user_online_presence\\\\\\\",\\\\\\\"video_upload\\\\\\\",\\\\\\\"xmpp_login\\\\\\\"]\\\",\\\"steps\\\":\\\"{}\\\",\\\"tp\\\":\\\"\\\\\\\"unspecified\\\\\\\"\\\",\\\"cui_gk\\\":\\\"\\\\\\\"[PASS]:\\\\\\\"\\\",\\\"is_limited_login_shim\\\":\\\"false\\\"}\",\"flow_name\":\"GDP\",\"flow_step_type\":\"STANDALONE\",\"outcome\":\"APPROVED\",\"source\":\"gdp_delegated\",\"surface\":\"FACEBOOK_COMET\"}}"
                                }
                            };
                            response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
                            MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
                            contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
                            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                            {
                                response.Content.Headers.ContentType.CharSet = "utf-8";
                            }
                            responseStr = await response.Content.ReadAsStringAsync();
                            string uri = JObject.Parse(responseStr)["data"]?["run_post_flow_action"]?["uri"]?.ToString();
                            if (string.IsNullOrEmpty(uri))
                            {
                                return (dtsgToken, token);
                            }
                            uri = HttpUtility.UrlDecode(uri);
                            token = Regex.Match(uri, "long_lived_token=(EAAG(.*?)ZDZD)").Groups[1].Value;
                            if (string.IsNullOrEmpty(token))
                            {
                                token = Regex.Match(responseStr, "(EAAG(.*?)ZD)").Value;
                            }
                        }
                        string lsdToken = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\"}").Groups[1].Value;
                        if (!string.IsNullOrEmpty(lsdToken))
                        {
                            _account.LSDToken = lsdToken;
                        }
                    }
                    else
                    {
                        response = await client.GetWithRetry("https://adsmanager.facebook.com/adsmanager/manage/campaigns?breakdown_regrouping=1&nav_source=no_referrer&act=", _account);
                        MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
                        contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
                        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                        {
                            response.Content.Headers.ContentType.CharSet = "utf-8";
                        }
                        HttpContent content = response.Content;
                        responseStr = await ((content != null) ? content.ReadAsStringAsync() : null);
                        token = Regex.Match(responseStr, "(EAA(.*?)ZDZD)").Value.Replace("\"", "").Replace("\\", "");
                        if (!string.IsNullOrEmpty(token))
                        {
                            _account.Token = token;
                        }
                        else
                        {
                            Dictionary<string, string> payload2 = new Dictionary<string, string>
                            {
                                { "av", _account.Uid },
                                { "__a", "1" },
                                { "dpr", "1" },
                                { "fb_dtsg", dtsgToken },
                                { "jazoest", "25668" },
                                { "lsd", "tbC0bJOdxzlMQiOsF3no8-" },
                                { "doc_id", "6494107973937368" },
                                {
                                    "variables",
                                    "{\"input\":{\"client_mutation_id\":\"4\",\"actor_id\":\"" + _account.Uid + "\",\"config_enum\":\"GDP_CONFIRM\",\"device_id\":null,\"experience_id\":\"ea0ce607-ab1d-4358-bdff-2dc18790cc5f\",\"extra_params_json\":\"{\\\"app_id\\\":\\\"436761779744620\\\",\\\"kid_directed_site\\\":\\\"false\\\",\\\"logger_id\\\":\\\"\\\\\\\"454a99cc-0d5a-4ed5-916b-987047ece6ce\\\\\\\"\\\",\\\"next\\\":\\\"\\\\\\\"confirm\\\\\\\"\\\",\\\"redirect_uri\\\":\\\"\\\\\\\"https:\\\\\\\\\\\\/\\\\\\\\\\\\/www.facebook.com\\\\\\\\\\\\/connect\\\\\\\\\\\\/login_success.html\\\\\\\"\\\",\\\"response_type\\\":\\\"\\\\\\\"token\\\\\\\"\\\",\\\"return_scopes\\\":\\\"false\\\",\\\"scope\\\":\\\"[\\\\\\\"user_subscriptions\\\\\\\",\\\\\\\"user_videos\\\\\\\",\\\\\\\"user_website\\\\\\\",\\\\\\\"user_work_history\\\\\\\",\\\\\\\"friends_about_me\\\\\\\",\\\\\\\"friends_actions.books\\\\\\\",\\\\\\\"friends_actions.music\\\\\\\",\\\\\\\"friends_actions.news\\\\\\\",\\\\\\\"friends_actions.video\\\\\\\",\\\\\\\"friends_activities\\\\\\\",\\\\\\\"friends_birthday\\\\\\\",\\\\\\\"friends_education_history\\\\\\\",\\\\\\\"friends_events\\\\\\\",\\\\\\\"friends_games_activity\\\\\\\",\\\\\\\"friends_groups\\\\\\\",\\\\\\\"friends_hometown\\\\\\\",\\\\\\\"friends_interests\\\\\\\",\\\\\\\"friends_likes\\\\\\\",\\\\\\\"friends_location\\\\\\\",\\\\\\\"friends_notes\\\\\\\",\\\\\\\"friends_photos\\\\\\\",\\\\\\\"friends_questions\\\\\\\",\\\\\\\"friends_relationship_details\\\\\\\",\\\\\\\"friends_relationships\\\\\\\",\\\\\\\"friends_religion_politics\\\\\\\",\\\\\\\"friends_status\\\\\\\",\\\\\\\"friends_subscriptions\\\\\\\",\\\\\\\"friends_videos\\\\\\\",\\\\\\\"friends_website\\\\\\\",\\\\\\\"friends_work_history\\\\\\\",\\\\\\\"ads_management\\\\\\\",\\\\\\\"create_event\\\\\\\",\\\\\\\"create_note\\\\\\\",\\\\\\\"export_stream\\\\\\\",\\\\\\\"friends_online_presence\\\\\\\",\\\\\\\"manage_friendlists\\\\\\\",\\\\\\\"manage_notifications\\\\\\\",\\\\\\\"manage_pages\\\\\\\",\\\\\\\"photo_upload\\\\\\\",\\\\\\\"publish_stream\\\\\\\",\\\\\\\"read_friendlists\\\\\\\",\\\\\\\"read_insights\\\\\\\",\\\\\\\"read_mailbox\\\\\\\",\\\\\\\"read_page_mailboxes\\\\\\\",\\\\\\\"read_requests\\\\\\\",\\\\\\\"read_stream\\\\\\\",\\\\\\\"rsvp_event\\\\\\\",\\\\\\\"share_item\\\\\\\",\\\\\\\"sms\\\\\\\",\\\\\\\"status_update\\\\\\\",\\\\\\\"user_online_presence\\\\\\\",\\\\\\\"video_upload\\\\\\\",\\\\\\\"xmpp_login\\\\\\\"]\\\",\\\"steps\\\":\\\"{}\\\",\\\"tp\\\":\\\"\\\\\\\"unspecified\\\\\\\"\\\",\\\"cui_gk\\\":\\\"\\\\\\\"[PASS]:\\\\\\\"\\\",\\\"is_limited_login_shim\\\":\\\"false\\\"}\",\"flow_name\":\"GDP\",\"flow_step_type\":\"STANDALONE\",\"outcome\":\"APPROVED\",\"source\":\"gdp_delegated\",\"surface\":\"FACEBOOK_COMET\"}}"
                                }
                            };
                            response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload2));
                            MediaTypeHeaderValue contentType5 = response.Content.Headers.ContentType;
                            contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
                            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                            {
                                response.Content.Headers.ContentType.CharSet = "utf-8";
                            }
                            responseStr = await response.Content.ReadAsStringAsync();
                            string uri2 = JObject.Parse(responseStr)["data"]?["run_post_flow_action"]?["uri"]?.ToString();
                            if (string.IsNullOrEmpty(uri2))
                            {
                                return (dtsgToken, token);
                            }
                            uri2 = HttpUtility.UrlDecode(uri2);
                            token = Regex.Match(uri2, "long_lived_token=(EAAG(.*?)ZDZD)").Groups[1].Value;
                            if (string.IsNullOrEmpty(token))
                            {
                                token = Regex.Match(responseStr, "(EAAG(.*?)ZD)").Value;
                            }
                        }
                        dtsgToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                        if (!string.IsNullOrEmpty(dtsgToken))
                        {
                            _account.DTSGToken = dtsgToken;
                        }
                        string lsdToken = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\"}").Groups[1].Value;
                        if (!string.IsNullOrEmpty(lsdToken))
                        {
                            _account.LSDToken = lsdToken;
                        }
                    }
                }
                dtsgToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                if (!string.IsNullOrEmpty(dtsgToken))
                {
                    _account.DTSGToken = dtsgToken;
                }
            }
            else
            {
                response = await client.GetWithRetry("https://adsmanager.facebook.com/adsmanager/manage/campaigns?breakdown_regrouping=1&nav_source=no_referrer&act=", _account);
                MediaTypeHeaderValue contentType6 = response.Content.Headers.ContentType;
                string contentType7 = ((contentType6 != null) ? contentType6.CharSet : null);
                if (!string.IsNullOrEmpty(contentType7) && contentType7.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                HttpContent content2 = response.Content;
                string responseStr2 = await ((content2 != null) ? content2.ReadAsStringAsync() : null);
                token = Regex.Match(responseStr2, "(EAA(.*?)ZDZD)").Value.Replace("\"", "").Replace("\\", "");
                if (!string.IsNullOrEmpty(token))
                {
                    _account.Token = token;
                }
                dtsgToken = Regex.Match(responseStr2, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                if (!string.IsNullOrEmpty(dtsgToken))
                {
                    _account.DTSGToken = dtsgToken;
                }
                string lsdToken = Regex.Match(responseStr2, "LSD\",\\[],{\"token\":\"(.*?)\"}").Groups[1].Value;
                if (!string.IsNullOrEmpty(lsdToken))
                {
                    _account.LSDToken = lsdToken;
                }
            }
            if (string.IsNullOrEmpty(token))
            {
                Dictionary<string, string> payload3 = new Dictionary<string, string>
                {
                    { "av", _account.Uid },
                    { "__a", "1" },
                    { "dpr", "1" },
                    { "fb_dtsg", dtsgToken },
                    { "jazoest", "25668" },
                    { "lsd", "tbC0bJOdxzlMQiOsF3no8-" },
                    { "doc_id", "6494107973937368" },
                    {
                        "variables",
                        "{\"input\":{\"client_mutation_id\":\"4\",\"actor_id\":\"" + _account.Uid + "\",\"config_enum\":\"GDP_CONFIRM\",\"device_id\":null,\"experience_id\":\"ea0ce607-ab1d-4358-bdff-2dc18790cc5f\",\"extra_params_json\":\"{\\\"app_id\\\":\\\"436761779744620\\\",\\\"kid_directed_site\\\":\\\"false\\\",\\\"logger_id\\\":\\\"\\\\\\\"454a99cc-0d5a-4ed5-916b-987047ece6ce\\\\\\\"\\\",\\\"next\\\":\\\"\\\\\\\"confirm\\\\\\\"\\\",\\\"redirect_uri\\\":\\\"\\\\\\\"https:\\\\\\\\\\\\/\\\\\\\\\\\\/www.facebook.com\\\\\\\\\\\\/connect\\\\\\\\\\\\/login_success.html\\\\\\\"\\\",\\\"response_type\\\":\\\"\\\\\\\"token\\\\\\\"\\\",\\\"return_scopes\\\":\\\"false\\\",\\\"scope\\\":\\\"[\\\\\\\"user_subscriptions\\\\\\\",\\\\\\\"user_videos\\\\\\\",\\\\\\\"user_website\\\\\\\",\\\\\\\"user_work_history\\\\\\\",\\\\\\\"friends_about_me\\\\\\\",\\\\\\\"friends_actions.books\\\\\\\",\\\\\\\"friends_actions.music\\\\\\\",\\\\\\\"friends_actions.news\\\\\\\",\\\\\\\"friends_actions.video\\\\\\\",\\\\\\\"friends_activities\\\\\\\",\\\\\\\"friends_birthday\\\\\\\",\\\\\\\"friends_education_history\\\\\\\",\\\\\\\"friends_events\\\\\\\",\\\\\\\"friends_games_activity\\\\\\\",\\\\\\\"friends_groups\\\\\\\",\\\\\\\"friends_hometown\\\\\\\",\\\\\\\"friends_interests\\\\\\\",\\\\\\\"friends_likes\\\\\\\",\\\\\\\"friends_location\\\\\\\",\\\\\\\"friends_notes\\\\\\\",\\\\\\\"friends_photos\\\\\\\",\\\\\\\"friends_questions\\\\\\\",\\\\\\\"friends_relationship_details\\\\\\\",\\\\\\\"friends_relationships\\\\\\\",\\\\\\\"friends_religion_politics\\\\\\\",\\\\\\\"friends_status\\\\\\\",\\\\\\\"friends_subscriptions\\\\\\\",\\\\\\\"friends_videos\\\\\\\",\\\\\\\"friends_website\\\\\\\",\\\\\\\"friends_work_history\\\\\\\",\\\\\\\"ads_management\\\\\\\",\\\\\\\"create_event\\\\\\\",\\\\\\\"create_note\\\\\\\",\\\\\\\"export_stream\\\\\\\",\\\\\\\"friends_online_presence\\\\\\\",\\\\\\\"manage_friendlists\\\\\\\",\\\\\\\"manage_notifications\\\\\\\",\\\\\\\"manage_pages\\\\\\\",\\\\\\\"photo_upload\\\\\\\",\\\\\\\"publish_stream\\\\\\\",\\\\\\\"read_friendlists\\\\\\\",\\\\\\\"read_insights\\\\\\\",\\\\\\\"read_mailbox\\\\\\\",\\\\\\\"read_page_mailboxes\\\\\\\",\\\\\\\"read_requests\\\\\\\",\\\\\\\"read_stream\\\\\\\",\\\\\\\"rsvp_event\\\\\\\",\\\\\\\"share_item\\\\\\\",\\\\\\\"sms\\\\\\\",\\\\\\\"status_update\\\\\\\",\\\\\\\"user_online_presence\\\\\\\",\\\\\\\"video_upload\\\\\\\",\\\\\\\"xmpp_login\\\\\\\"]\\\",\\\"steps\\\":\\\"{}\\\",\\\"tp\\\":\\\"\\\\\\\"unspecified\\\\\\\"\\\",\\\"cui_gk\\\":\\\"\\\\\\\"[PASS]:\\\\\\\"\\\",\\\"is_limited_login_shim\\\":\\\"false\\\"}\",\"flow_name\":\"GDP\",\"flow_step_type\":\"STANDALONE\",\"outcome\":\"APPROVED\",\"source\":\"gdp_delegated\",\"surface\":\"FACEBOOK_COMET\"}}"
                    }
                };
                response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload3));
                MediaTypeHeaderValue contentType8 = response.Content.Headers.ContentType;
                string contentType9 = ((contentType8 != null) ? contentType8.CharSet : null);
                if (!string.IsNullOrEmpty(contentType9) && contentType9.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                string responseStr3 = await response.Content.ReadAsStringAsync();
                string uri3 = JObject.Parse(responseStr3)["data"]?["run_post_flow_action"]?["uri"]?.ToString();
                if (string.IsNullOrEmpty(uri3))
                {
                    return (dtsgToken, token);
                }
                uri3 = HttpUtility.UrlDecode(uri3);
                token = Regex.Match(uri3, "long_lived_token=(EAAG(.*?)ZDZD)").Groups[1].Value;
                if (string.IsNullOrEmpty(token))
                {
                    token = Regex.Match(responseStr3, "(EAAG(.*?)ZD)").Value;
                }
            }
        }
        catch
        {
        }
        return (dtsgToken, token);
    }

    public async Task<List<BusinessManagermentDto>> LoadAllBM()
    {
        List<BusinessManagermentDto> listBusiness = new List<BusinessManagermentDto>();
        _ = string.Empty;
        if (string.IsNullOrEmpty(_account.Token))
        {
            (_account.DTSGToken, _account.Token) = await GetTokenAsync();
        }
        if (string.IsNullOrEmpty(_account.Token))
        {
            return null;
        }
        string nextUrl = "https://graph.facebook.com/v14.0/me/businesses?fields=id,name,created_time,timezone_id,verification_status,primary_page{name,id},allow_page_management_in_www,sharing_eligibility_status,can_create_ad_account,limit{1000}&access_token=" + _account.Token;
        try
        {
            do
            {
                try
                {
                    HttpResponseMessage response = await client.GetWithRetry(nextUrl, _account);
                    if (!response.IsSuccessStatusCode)
                    {
                        return listBusiness;
                    }
                    JObject dataObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                    JToken businessDataObjects = dataObject["data"];
                    foreach (JToken businessDataObject in (IEnumerable<JToken>)businessDataObjects)
                    {
                        try
                        {
                            JToken businessObjectTemp = businessDataObject;
                            string businessId = businessObjectTemp["id"].ToString();
                            string businessName = businessObjectTemp["name"].ToString();
                            string businessType = ((businessObjectTemp["sharing_eligibility_status"]?.ToString() == "enabled") ? "BM350" : "BM50");
                            string businessVerifyStatus = ((businessObjectTemp["verification_status"]?.ToString() == "not_verified") ? "Chưa xác minh" : "Đã xác minh");
                            string businessCanCreateAccount = ((businessObjectTemp["can_create_ad_account"]?.ToString() == "True") ? "Được tạo" : "Không được tạo");
                            string businessPrimaryPage = businessObjectTemp["primary_page"]?["name"]?.ToString();
                            string businessTimeZoneId = businessObjectTemp["timezone_id"]?.ToString();
                            string businessCreatedTime = Convert.ToDateTime(businessObjectTemp["created_time"]?.ToString()).ToString("dd/MM/yyyy");
                            string businessStatus = ((businessObjectTemp["allow_page_management_in_www"]?.ToString() == "True") ? "BM Live" : "BM Die");
                            BusinessInfomationDto businessDto = new BusinessInfomationDto
                            {
                                BusinessId = businessId,
                                BusinessName = businessName,
                                BusinessType = businessType,
                                BusinessVerified = businessVerifyStatus,
                                CanCreateAccount = businessCanCreateAccount,
                                PrimaryPage = businessPrimaryPage,
                                TimeZoneId = businessTimeZoneId,
                                CreateTime = businessCreatedTime,
                                StatusBusiness = businessStatus
                            };
                            BusinessManagermentDto businessManagermentDto = new BusinessManagermentDto
                            {
                                BusinessInfo = businessDto
                            };
                            listBusiness.Add(businessManagermentDto);
                        }
                        catch
                        {
                        }
                    }
                    nextUrl = dataObject["paging"]?["next"]?.ToString();
                    await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 3)));
                }
                catch
                {
                }
            }
            while (!string.IsNullOrEmpty(nextUrl));
        }
        catch
        {
        }
        return listBusiness;
    }

    public async Task<FriendStatus> UnFriendRequest(string uid)
    {
        try
        {
            if (string.IsNullOrEmpty(_account.DTSGToken))
            {
                HttpResponseMessage responseProfile = await client.GetAsync("https://www.facebook.com/me");
                if (!responseProfile.IsSuccessStatusCode)
                {
                    return FriendStatus.Error;
                }
                MediaTypeHeaderValue contentType = responseProfile.Content.Headers.ContentType;
                string contentType2 = ((contentType != null) ? contentType.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    responseProfile.Content.Headers.ContentType.CharSet = "utf-8";
                }
                string responseProfileStr = await responseProfile.Content.ReadAsStringAsync();
                _account.DTSGToken = Regex.Match(responseProfileStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
            }
            HttpResponseMessage response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "fb_dtsg", _account.DTSGToken },
                { "__a", "1" },
                { "dpr", "1" },
                { "jazoest", "25785" },
                { "lsd", "5EwkixPmz7tpyzQia29R7G" },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "FriendingCometUnfriendMutation" },
                {
                    "variables",
                    "{\"input\":{\"source\":\"friending_jewel\",\"unfriended_user_id\":\"" + uid + "\",\"actor_id\":\"" + _account.Uid + "\",\"client_mutation_id\":\"3\"},\"scale\":1}"
                },
                { "server_timestamps", "true" },
                { "doc_id", "23930708339886851" }
            }), client: client, url: "https://www.facebook.com/api/graphql/", account: _account);
            if (!response.IsSuccessStatusCode)
            {
                return FriendStatus.Error;
            }
            MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
            string contentType4 = ((contentType3 != null) ? contentType3.CharSet : null);
            if (!string.IsNullOrEmpty(contentType4) && contentType4.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("CAN_REQUEST") || responseStr.Contains("OUTGOING_REQUEST"))
            {
                return FriendStatus.Unfriend;
            }
            if (responseStr.Contains("code\":1675030"))
            {
                FacebookHandler facebookHandler = new FacebookHandler(new AccountDto
                {
                    Uid = _account.Uid,
                    Password = _account.Password,
                    Cookie = _account.Cookie
                }, _userAgent, _proxy);
                if (await facebookHandler.LoginFacebook(1) != StatusCookie.Live)
                {
                    return FriendStatus.Checkpoint;
                }
                return await facebookHandler.UnFriendRequest(uid);
            }
            if (responseStr.Contains("checkpoint/1501092823525282"))
            {
                return FriendStatus.Checkpoint;
            }
        }
        catch
        {
        }
        return FriendStatus.Error;
    }

    public async Task<bool> ShareTKQC(string adAccountId, string uid, int typeApiVersion = 0)
    {
        try
        {
            string token = (await GetTokenAsync()).Item2;
            if (!string.IsNullOrEmpty(token))
            {
                _account.Token = token;
            }
            HttpResponseMessage response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
            {
                { "account_id", adAccountId },
                { "include_headers", "false" },
                { "locale", "en_GB" },
                { "method", "post" },
                { "pretty", "0" },
                { "role", "281423141961500" },
                { "suppress_http_code", "1" },
                { "uid", uid },
                { "xref", "fd6552ba51a024e83" }
            }), client: client, url: "https://adsmanager-graph.facebook.com/v15.0/act_" + adAccountId + "/users?_reqName=adaccount%2Fusers&access_token=" + _account.Token, account: _account);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("code\":10"))
            {
                return await APIShareTKQC2(uid, adAccountId);
            }
            if (responseStr.Contains("\"code\":190"))
            {
                FacebookHandler facebookHandler = new FacebookHandler(new AccountDto
                {
                    Uid = _account.Uid,
                    Password = _account.Password,
                    Cookie = _account.Cookie
                }, _userAgent, _proxy);
                if (await facebookHandler.LoginFacebook(1) != StatusCookie.Live)
                {
                    return false;
                }
                return await facebookHandler.ShareTKQC(adAccountId, uid);
            }
            if (responseStr.Contains("\"success\":true"))
            {
                return true;
            }
        }
        catch
        {
        }
        return false;
    }

    private async Task<bool> APIShareTKQC2(string uid, string adAccountId)
    {
        try
        {
            HttpResponseMessage response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
            {
                { "account_id", adAccountId },
                { "__a", "1" },
                { "include_headers", "false" },
                { "locale", "en_GB" },
                { "method", "post" },
                { "pretty", "0" },
                { "role", "281423141961500" },
                { "suppress_http_code", "1" },
                { "uid", uid },
                { "xref", "f2526fbf53d4414" }
            }), client: client, url: "https://adsmanager-graph.facebook.com/v15.0/act_" + adAccountId + "/users?_reqName=adaccount%2Fusers&access_token=" + _account.Token + "&method=post&__cppo=1", account: _account);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            string responseStr = ((object)response)?.ToString();
            if (responseStr.Contains("success"))
            {
                return true;
            }
        }
        catch
        {
        }
        return false;
    }

    public async Task<(List<AdAccountDto>, string)> LoadAdAccountV2(string businessId, int typeLoad, string nextUrl = "")
    {
        AccountDto account = _account;
        AccountDto account2 = _account;
        (string, string) tuple = await GetTokenAsync();
        account.DTSGToken = tuple.Item1;
        account2.Token = tuple.Item2;
        ConcurrentBag<AdAccountDto> adAccountDtos = new ConcurrentBag<AdAccountDto>();
        if (typeLoad == 0)
        {
            if (string.IsNullOrEmpty(nextUrl))
            {
                nextUrl = "https://graph.facebook.com/v17.0/" + businessId + "/client_ad_accounts?access_token=" + _account.Token + "&__activeScenarioIDs=[]&__activeScenarios=[]&__interactionsMetadata=[]&_reqName=object:business/client_ad_accounts&_reqSrc=BusinessConnectedClientAdAccountsStore.brands&date_format=U&fields=[\"business_country_code\",\"owner\",\"timezone_offset_hours_utc\",\"business\",\"agencies\",\"userpermissions\",\"spend_cap\",\"created_time\",\"funding_source_details\",\"campaigns{boosted_object_id}\",\"account_status\",\"adspaymentcycle\",\"id\",\"currency\",\"amount_spent\",\"balance\",\"name\",\"timezone_name\",\"adtrust_dsl\",\"disable_reason\",\"min_billing_threshold\"]&limit=50&locale=en_GB&method=get&pretty=0&sort=name_ascending&suppress_http_code=1&xref=fc43554762568c76b";
            }
        }
        else if (string.IsNullOrEmpty(nextUrl))
        {
            nextUrl = "https://graph.facebook.com/v17.0/" + businessId + "/owned_ad_accounts?access_token=" + _account.Token + "&__activeScenarioIDs=[]&__activeScenarios=[]&__interactionsMetadata=[]&_reqName=object:business/owned_ad_accounts&_reqSrc=BusinessConnectedOwnedAdAccountsStore.brands&date_format=U&fields=[\"business_country_code\",\"owner\",\"timezone_offset_hours_utc\",\"business\",\"agencies\",\"userpermissions\",\"spend_cap\",\"created_time\",\"funding_source_details\",\"campaigns{boosted_object_id}\",\"account_status\",\"adspaymentcycle\",\"id\",\"currency\",\"amount_spent\",\"balance\",\"name\",\"timezone_name\",\"adtrust_dsl\",\"disable_reason\",\"min_billing_threshold\"]&limit=50&locale=en_GB&method=get&pretty=0&sort=name_ascending&suppress_http_code=1&xref=f06badf73fa916dd1&_callFlowletID=0&_triggerFlowletID=11150";
        }
        try
        {
            HttpResponseMessage response = await client.GetAsync(nextUrl);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            if (((contentType != null) ? contentType.CharSet : null)?.Contains("\"utf-8\"") ?? false)
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            JObject adAccountObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            JArray adAccountJArr = adAccountObject["data"].ToObject<JArray>();
            foreach (JObject adAccountJArrObject in adAccountJArr.Cast<JObject>())
            {
                string users = string.Join("|", (from x in adAccountJArrObject["userpermissions"]?["data"]?.Where((JToken x) => x["user"]?["id"] != null)
                                                 select x["user"]["id"].ToString()) ?? Enumerable.Empty<string>());
                string partnerIds = string.Join("|", (from x in adAccountJArrObject["agencies"]?["data"]?.Where((JToken x) => x["id"] != null)
                                                      select x["id"].ToString()) ?? Enumerable.Empty<string>());
                AdAccountDto adAccountDto = new AdAccountDto
                {
                    AccountId = adAccountJArrObject["id"].ToString().Replace("act_", ""),
                    AccountName = adAccountJArrObject["name"].ToString(),
                    Currency = adAccountJArrObject["currency"].ToString(),
                    SpendCap = (double.Parse(adAccountJArrObject["spend_cap"]?.ToString()) / 100.0).ToString(),
                    AccountLimit = ((adAccountJArrObject["adtrust_dsl"].ToString() == "-1") ? "Nolimit" : adAccountJArrObject["adtrust_dsl"].ToString()),
                    AccountThreshold = adAccountJArrObject["min_billing_threshold"]?["amount"]?.ToString(),
                    AccountBalance = adAccountJArrObject["balance"]?.ToString(),
                    AccountSpent = adAccountJArrObject["amount_spent"]?.ToString(),
                    PaymentMethod = (string.IsNullOrEmpty(adAccountJArrObject["funding_source_details"]?["display_string"]?.ToString()) ? "N/A" : adAccountJArrObject["funding_source_details"]?["display_string"]?.ToString()),
                    TimeZone = adAccountJArrObject["timezone_name"].ToString() + "/" + adAccountJArrObject["timezone_offset_hours_utc"],
                    CampaignCount = (string.IsNullOrEmpty(adAccountJArrObject["campaigns"]?["data"]?.ToString()) ? "0" : adAccountJArrObject["campaigns"]?["data"]?.ToArray().Count().ToString()),
                    TypeAdAccount = (string.IsNullOrEmpty(adAccountJArrObject["business"]?.ToString()) ? "Cá nhân" : "Business"),
                    CreatedTime = (Common.IsDateTimeOffset(adAccountJArrObject["created_time"].ToString()) ? Convert.ToDateTime(adAccountJArrObject["created_time"].ToString()).ToString("dd/MM/yyyy") : DateTimeOffset.FromUnixTimeSeconds(long.Parse(adAccountJArrObject["created_time"].ToString())).DateTime.ToString("dd/MM/yyyy")),
                    Owner = adAccountJArrObject["owner"]?.ToString(),
                    BusinessCountryCode = adAccountJArrObject["business_country_code"]?.ToString(),
                    AdAccountStatus = ((adAccountJArrObject["account_status"]?.ToString() == "1") ? "Live" : ((adAccountJArrObject["account_status"]?.ToString() == "2") ? "Vô hiệu hóa" : ((adAccountJArrObject["account_status"]?.ToString() == "3") ? "Cần thanh toán" : ((adAccountJArrObject["account_status"]?.ToString() == "101") ? "Đóng" : ((adAccountJArrObject["account_status"]?.ToString() == "7") ? "PENDING_RISK_REVIEW" : ((adAccountJArrObject["account_status"]?.ToString() == "8") ? "PENDING_SETTLEMENT" : ((adAccountJArrObject["account_status"]?.ToString() == "9") ? "IN_GRACE_PERIOD" : ((adAccountJArrObject["account_status"]?.ToString() == "100") ? "PENDING_CLOSURE" : ((adAccountJArrObject["account_status"]?.ToString() == "201") ? "ANY_ACTIVE" : ((adAccountJArrObject["account_status"]?.ToString() == "202") ? "ANY_CLOSED" : "")))))))))),
                    Users = users,
                    Partners = partnerIds,
                    BusinessId = adAccountJArrObject["business"]?["id"]?.ToString()
                };
                adAccountDtos.Add(adAccountDto);
            }
            nextUrl = adAccountObject["paging"]?["next"]?.ToString();
        }
        catch
        {
            nextUrl = string.Empty;
        }
        return (adAccountDtos.ToList(), nextUrl);
    }

    public async Task<(List<AdAccountDto>, string)> LoadAdAccountV2(string businessId, int typeLoad, int filter, string nextUrl = "")
    {
        if (string.IsNullOrEmpty(_account.Token))
        {
            (_account.DTSGToken, _account.Token) = await GetTokenAsync();
        }
        ConcurrentBag<AdAccountDto> adAccountDtos = new ConcurrentBag<AdAccountDto>();
        if (typeLoad == 0)
        {
            if (string.IsNullOrEmpty(nextUrl))
            {
                nextUrl = $"https://graph.facebook.com/v17.0/{businessId}/client_ad_accounts?access_token={_account.Token}&__activeScenarioIDs=[]&__activeScenarios=[]&__interactionsMetadata=[]&_reqName=object:business/client_ad_accounts&_reqSrc=BusinessConnectedClientAdAccountsStore.brands&date_format=U&fields=[\"business_country_code\",\"owner\",\"timezone_offset_hours_utc\",\"business\",\"agencies\",\"userpermissions\",\"spend_cap\",\"created_time\",\"funding_source_details\",\"campaigns{{boosted_object_id}}\",\"account_status\",\"adspaymentcycle\",\"id\",\"currency\",\"amount_spent\",\"balance\",\"name\",\"timezone_name\",\"adtrust_dsl\",\"disable_reason\",\"min_billing_threshold\"]&filtering=[{{\"field\":\"account_status\",\"operator\":\"EQUAL\",\"value\":\"{filter}\"}}]&limit=50&locale=en_GB&method=get&pretty=0&sort=name_ascending&suppress_http_code=1&xref=fc43554762568c76b";
            }
        }
        else if (string.IsNullOrEmpty(nextUrl))
        {
            nextUrl = $"https://graph.facebook.com/v17.0/{businessId}/owned_ad_accounts?access_token={_account.Token}&__activeScenarioIDs=[]&__activeScenarios=[]&__interactionsMetadata=[]&_reqName=object:business/owned_ad_accounts&_reqSrc=BusinessConnectedOwnedAdAccountsStore.brands&date_format=U&fields=[\"business_country_code\",\"owner\",\"timezone_offset_hours_utc\",\"business\",\"agencies\",\"userpermissions\",\"spend_cap\",\"created_time\",\"funding_source_details\",\"campaigns{{boosted_object_id}}\",\"account_status\",\"adspaymentcycle\",\"id\",\"currency\",\"amount_spent\",\"balance\",\"name\",\"timezone_name\",\"adtrust_dsl\",\"disable_reason\",\"min_billing_threshold\"]&filtering=[{{\"field\":\"account_status\",\"operator\":\"EQUAL\",\"value\":\"{filter}\"}}]&limit=50&locale=en_GB&method=get&pretty=0&sort=name_ascending&suppress_http_code=1&xref=f06badf73fa916dd1&_callFlowletID=0&_triggerFlowletID=11150";
        }
        try
        {
            HttpResponseMessage response = await client.GetAsync(nextUrl);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            if (((contentType != null) ? contentType.CharSet : null)?.Contains("\"utf-8\"") ?? false)
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            JObject adAccountObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            JArray adAccountJArr = adAccountObject["data"].ToObject<JArray>();
            foreach (JObject adAccountJArrObject in adAccountJArr.Cast<JObject>())
            {
                string users = string.Join("|", (from x in adAccountJArrObject["userpermissions"]?["data"]?.Where((JToken x) => x["user"]?["id"] != null)
                                                 select x["user"]["id"].ToString()) ?? Enumerable.Empty<string>());
                string partnerIds = string.Join("|", (from x in adAccountJArrObject["userpermissions"]?["data"]?.Where((JToken x) => x["business"]?["id"] != null)
                                                      select x["business"]["id"].ToString()) ?? Enumerable.Empty<string>());
                adAccountJArrObject["name"].ToString();
                adAccountJArrObject["amount_spent"]?.ToString();
                (double.Parse(adAccountJArrObject["amount_spent"]?.ToString()) / 100.0).ToString();
                AdAccountDto adAccountDto = new AdAccountDto
                {
                    AccountId = adAccountJArrObject["id"].ToString().Replace("act_", ""),
                    AccountName = adAccountJArrObject["name"].ToString(),
                    Currency = adAccountJArrObject["currency"].ToString(),
                    SpendCap = (double.Parse(adAccountJArrObject["spend_cap"]?.ToString()) / 100.0).ToString(),
                    AccountLimit = ((adAccountJArrObject["adtrust_dsl"].ToString() == "-1") ? "Nolimit" : adAccountJArrObject["adtrust_dsl"].ToString()),
                    AccountThreshold = adAccountJArrObject["min_billing_threshold"]?["amount"]?.ToString(),
                    AccountBalance = (double.Parse(adAccountJArrObject["balance"]?.ToString()) / 100.0).ToString(),
                    AccountSpent = (double.Parse(adAccountJArrObject["amount_spent"]?.ToString()) / 100.0).ToString(),
                    PaymentMethod = (string.IsNullOrEmpty(adAccountJArrObject["funding_source_details"]?["display_string"]?.ToString()) ? "N/A" : adAccountJArrObject["funding_source_details"]?["display_string"]?.ToString()),
                    TimeZone = adAccountJArrObject["timezone_name"].ToString() + "/" + adAccountJArrObject["timezone_offset_hours_utc"],
                    CampaignCount = (string.IsNullOrEmpty(adAccountJArrObject["campaigns"]?["data"]?.ToString()) ? "0" : adAccountJArrObject["campaigns"]?["data"]?.ToArray().Count().ToString()),
                    TypeAdAccount = (string.IsNullOrEmpty(adAccountJArrObject["business"]?.ToString()) ? "Cá nhân" : "Business"),
                    CreatedTime = (Common.IsDateTimeOffset(adAccountJArrObject["created_time"].ToString()) ? Convert.ToDateTime(adAccountJArrObject["created_time"].ToString()).ToString("dd/MM/yyyy") : DateTimeOffset.FromUnixTimeSeconds(long.Parse(adAccountJArrObject["created_time"].ToString())).DateTime.ToString("dd/MM/yyyy")),
                    Owner = adAccountJArrObject["owner"]?.ToString(),
                    BusinessCountryCode = adAccountJArrObject["business_country_code"]?.ToString(),
                    AdAccountStatus = ((adAccountJArrObject["account_status"]?.ToString() == "1") ? "Live" : ((adAccountJArrObject["account_status"]?.ToString() == "2") ? "Vô hiệu hóa" : ((adAccountJArrObject["account_status"]?.ToString() == "3") ? "Cần thanh toán" : ((adAccountJArrObject["account_status"]?.ToString() == "101") ? "Đóng" : ((adAccountJArrObject["account_status"]?.ToString() == "7") ? "PENDING_RISK_REVIEW" : ((adAccountJArrObject["account_status"]?.ToString() == "8") ? "PENDING_SETTLEMENT" : ((adAccountJArrObject["account_status"]?.ToString() == "9") ? "IN_GRACE_PERIOD" : ((adAccountJArrObject["account_status"]?.ToString() == "100") ? "PENDING_CLOSURE" : ((adAccountJArrObject["account_status"]?.ToString() == "201") ? "ANY_ACTIVE" : ((adAccountJArrObject["account_status"]?.ToString() == "202") ? "ANY_CLOSED" : "")))))))))),
                    Users = users,
                    Partners = partnerIds,
                    BusinessId = adAccountJArrObject["business"]?["id"]?.ToString()
                };
                adAccountDtos.Add(adAccountDto);
            }
            nextUrl = adAccountObject["paging"]?["next"]?.ToString();
        }
        catch
        {
            nextUrl = string.Empty;
        }
        return (adAccountDtos.ToList(), nextUrl);
    }

    public async Task<List<AdAccountDto>> LoadTKQCHide(string businessId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "fb_dtsg", _account.DTSGToken },
            { "lsd", "K8SDtR_J7dfSEUsG1fxIav" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "AccountQualityBusinessAdAccountsWrapper_AllBusinessAdAccountsQuery" },
            { "server_timestamps", "true" },
            { "doc_id", "9321940464601723" },
            {
                "variables",
                $"{{\"assetOwnerId\":\"{businessId}\",\"startTime\":{DateTimeOffset.Now.ToUnixTimeSeconds()},\"count\":25,\"cursor\":null}}"
            }
        };
        List<AdAccountDto> adAccountDtos = new List<AdAccountDto>();
        string nextUrl = "https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=14947&qpl_active_e2e_trace_ids=";
        do
        {
            try
            {
                HttpResponseMessage response = await client.PostAsync(nextUrl, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
                if (!response.IsSuccessStatusCode)
                {
                    return adAccountDtos;
                }
                MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
                if (((contentType != null) ? contentType.CharSet : null)?.Contains("\"utf-8\"") ?? false)
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                JObject adAccountObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                JToken adAccountDataJArr = adAccountObject["data"];
                JArray adAccountJArr = adAccountDataJArr["assetOwnerData"]["all_business_ad_accounts"]["edges"].ToObject<JArray>();
                foreach (JObject adAccountJArrObject in adAccountJArr)
                {
                    JToken node = adAccountJArrObject["node"];
                    AdAccountDto adAccountDto = new AdAccountDto
                    {
                        AccountId = node["id"].ToString().Replace("act_", ""),
                        AccountName = node["name"].ToString(),
                        Currency = node["currency"]?.ToString(),
                        SpendCap = node["spend_cap"]?.ToString(),
                        AccountLimit = ((node["adtrust_dsl"]?.ToString() == "-1") ? "Nolimit" : node["adtrust_dsl"]?.ToString())
                    };
                    adAccountDtos.Add(adAccountDto);
                }
                nextUrl = adAccountObject["paging"]?["next"]?.ToString();
            }
            catch
            {
                nextUrl = string.Empty;
            }
        }
        while (!string.IsNullOrEmpty(nextUrl));
        return adAccountDtos;
    }

    public async Task<(List<AdAccountDto>, string)> LoadAdAccountPersonV2()
    {
        if (string.IsNullOrEmpty(_account.Token))
        {
            (_account.DTSGToken, _account.Token) = await GetTokenAsync();
        }
        List<AdAccountDto> adAccountDtos = new List<AdAccountDto>();
        string nextUrl = "https://graph.facebook.com/me/adaccounts?fields=agencies,business,business_country_code,owner,spend_cap,created_time,funding_source_details,timezone_offset_hours_utc,all_payment_methods{pm_credit_card{display_string}},campaigns{boosted_object_id},account_status,adspaymentcycle,id,currency,amount_spent,balance,name,timezone_name,userpermissions.users(" + _account.Uid + "),adtrust_dsl,disable_reason,min_billing_threshold,payment_method_tokens{type}&limit=50&summary=total_count&access_token=" + _account.Token;
        try
        {
            HttpResponseMessage response = await client.GetAsync(nextUrl);
            if (!response.IsSuccessStatusCode)
            {
                return (adAccountDtos, null);
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            if (((contentType != null) ? contentType.CharSet : null)?.Contains("\"utf-8\"") ?? false)
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            JObject adAccountObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            JArray adAccountJArr = adAccountObject["data"].ToObject<JArray>();
            foreach (JObject adAccountJArrObject in adAccountJArr)
            {
                string users = string.Join("|", (from x in adAccountJArrObject["userpermissions"]?["data"]?.Where((JToken x) => x["user"]?["id"] != null)
                                                 select x["user"]["id"].ToString()) ?? Enumerable.Empty<string>());
                string partnerIds = string.Join("|", (from x in adAccountJArrObject["agencies"]?["data"]?.Where((JToken x) => x["id"] != null)
                                                      select x["id"].ToString()) ?? Enumerable.Empty<string>());
                int typeStatus = 0;
                string statusAdAccount = string.Empty;
                switch (adAccountJArrObject["account_status"]?.ToString())
                {
                    case "1":
                        typeStatus = 1;
                        statusAdAccount = "Live";
                        break;
                    case "2":
                        typeStatus = 2;
                        statusAdAccount = (adAccountJArrObject["disable_reason"]?.ToString() ?? "") switch
                        {
                            "0" => "Vô hiệu hóa=>NONE",
                            "1" => "Vô hiệu hóa=>ADS_INTEGRITY_POLICY",
                            "2" => "Vô hiệu hóa=>ADS_IP_REVIEW",
                            "3" => "Vô hiệu hóa=>RISK_PAYMENT",
                            "4" => "Vô hiệu hóa=>GRAY_ACCOUNT_SHUT_DOWN",
                            "5" => "Vô hiệu hóa=>ADS_AFC_REVIEW",
                            "6" => "Vô hiệu hóa=>BUSINESS_INTEGRITY_RAR",
                            "7" => "Vô hiệu hóa=>PERMANENT_CLOSE",
                            "8" => "Vô hiệu hóa=>UNUSED_RESELLER_ACCOUNT",
                            "9" => "Vô hiệu hóa=>UNUSED_ACCOUNT",
                            "10" => "Vô hiệu hóa=>UMBRELLA_AD_ACCOUNT",
                            "11" => "Vô hiệu hóa=>BUSINESS_MANAGER_INTEGRITY_POLICY",
                            "12" => "Vô hiệu hóa=>MISREPRESENTED_AD_ACCOUNT",
                            "13" => "Vô hiệu hóa=>AOAB_DESHARE_LEGAL_ENTITY",
                            "14" => "Vô hiệu hóa=>CTX_THREAD_REVIEW",
                            "15" => "Vô hiệu hóa=>COMPROMISED_AD_ACCOUNT",
                            _ => "Vô hiệu hóa=>Unknown",
                        };
                        break;
                    case "3":
                        typeStatus = 3;
                        statusAdAccount = "Cần thanh toán";
                        break;
                    case "101":
                        statusAdAccount = "Đóng";
                        break;
                }
                AdAccountDto adAccountDto = new AdAccountDto
                {
                    AccountId = adAccountJArrObject["id"].ToString().Replace("act_", ""),
                    AccountName = adAccountJArrObject["name"].ToString(),
                    Currency = adAccountJArrObject["currency"].ToString(),
                    SpendCap = (double.Parse(adAccountJArrObject["spend_cap"]?.ToString()) / 100.0).ToString(),
                    AccountLimit = ((adAccountJArrObject["adtrust_dsl"].ToString() == "-1") ? "Nolimit" : adAccountJArrObject["adtrust_dsl"].ToString()),
                    AccountThreshold = adAccountJArrObject["min_billing_threshold"]?["amount"]?.ToString(),
                    AccountBalance = adAccountJArrObject["balance"]?.ToString(),
                    AccountSpent = (double.Parse(adAccountJArrObject["amount_spent"]?.ToString()) / 100.0).ToString(),
                    PaymentMethod = (string.IsNullOrEmpty(adAccountJArrObject["funding_source_details"]?["display_string"]?.ToString()) ? "N/A" : adAccountJArrObject["funding_source_details"]?["display_string"]?.ToString()),
                    TimeZone = adAccountJArrObject["timezone_name"].ToString() + "/" + adAccountJArrObject["timezone_offset_hours_utc"],
                    CampaignCount = (string.IsNullOrEmpty(adAccountJArrObject["campaigns"]?["data"]?.ToString()) ? "0" : adAccountJArrObject["campaigns"]?["data"]?.ToArray().Count().ToString()),
                    TypeAdAccount = (string.IsNullOrEmpty(adAccountJArrObject["business"]?.ToString()) ? "Cá nhân" : "Business"),
                    CreatedTime = (Common.IsDateTimeOffset(adAccountJArrObject["created_time"].ToString()) ? Convert.ToDateTime(adAccountJArrObject["created_time"].ToString()).ToString("dd/MM/yyyy") : DateTimeOffset.FromUnixTimeSeconds(long.Parse(adAccountJArrObject["created_time"].ToString())).DateTime.ToString("dd/MM/yyyy")),
                    Owner = adAccountJArrObject["owner"]?.ToString(),
                    BusinessCountryCode = adAccountJArrObject["business_country_code"]?.ToString(),
                    AdAccountStatus = statusAdAccount,
                    Users = users,
                    Partners = partnerIds,
                    BusinessId = adAccountJArrObject["business"]?["id"]?.ToString(),
                    TypeStatus = typeStatus
                };
                adAccountDtos.Add(adAccountDto);
            }
            nextUrl = adAccountObject["paging"]?["next"]?.ToString();
        }
        catch
        {
            nextUrl = string.Empty;
        }
        return (adAccountDtos, nextUrl);
    }

    public async Task<List<BusinessUserDto>> LoadUser(string businessId)
    {
        List<BusinessUserDto> users = new List<BusinessUserDto>();
        string nextUrl = "https://graph.facebook.com/graphql?method=post&locale=en_US&pretty=false&format=json&fb_api_caller_class=RelayModern&fb_api_req_friendly_name=BizKitSettingsPeopleTableListPaginationQuery&variables={\"asset_types\":null,\"businessAccessType\":[],\"businessAccountTypes\":[],\"cursor\":null,\"first\":25,\"isBulkUserRemovalEnabled\":true,\"isUnifiedSettings\":true,\"orderBy\":\"MOST_RECENTLY_CREATED\",\"permissions\":[],\"searchTerm\":null,\"id\":\"" + businessId + "\"}&server_timestamps=true&doc_id=9371006629693295&access_token=" + _account.Token;
        do
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(nextUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return users;
                }
                JObject jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                JArray userDatas = jObject["data"]?["node"]?["business_users_and_invitations"]?["edges"].ToObject<JArray>();
                foreach (JObject userData in userDatas)
                {
                    JObject userInfo = userData["userInfoForSelection"].ToObject<JObject>();
                    string role = userData["roleColumn"]?["permitted_business_account_tasks_summary"]?["standalone"]?["primary_access_summary"].ToString();
                    users.Add(new BusinessUserDto
                    {
                        UserId = userInfo["id"]?.ToString(),
                        UserName = userInfo["name"]?.ToString(),
                        UserRole = role,
                        UserEmail = userInfo["email"]?.ToString()
                    });
                }
                nextUrl = jObject["paging"]?["next"]?.ToString();
            }
            catch
            {
                nextUrl = string.Empty;
            }
        }
        while (!string.IsNullOrEmpty(nextUrl));
        return users;
    }

    public async Task<string> CheckLimitBusiness(string businessId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25248" },
            { "lsd", "TPpBv0V3Dd0-XHr7uEhwEG" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/business/adaccount/limits/?business_id=" + businessId, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string limitBM = Regex.Match(await response.Content.ReadAsStringAsync(), "adAccountLimit\":(.*?)}").Groups[1].Value;
        if (string.IsNullOrEmpty(limitBM))
        {
            response = await client.GetAsync("https://business.facebook.com/latest/settings/business_info?business_id=" + businessId);
            MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
            contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            limitBM = Regex.Match(await response.Content.ReadAsStringAsync(), "\"ad_account_creation_limit\":\\s*(\\d+)").Groups[1].Value;
        }
        return limitBM;
    }

    public async Task<BusinessManagermentDto> CheckBusinessInfomation(string businessId)
    {
        string url = "https://graph.facebook.com/v14.0/" + businessId + "?fields=id,name,business_users,created_time,timezone_id,verification_status,primary_page{name,id},allow_page_management_in_www,sharing_eligibility_status,can_create_ad_account,owned_ad_accounts.summary(1).limit(0),client_ad_accounts.summary(1).limit(0)&access_token=" + _account.Token;
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        JObject businessDataObjects = JObject.Parse(await response.Content.ReadAsStringAsync());
        try
        {
            JObject bodyObject = businessDataObjects;
            string businessName = bodyObject["name"].ToString();
            string businessType = ((bodyObject["sharing_eligibility_status"]?.ToString() == "enabled") ? "BM350" : "BM50");
            string businessVerifyStatus = ((bodyObject["verification_status"]?.ToString() == "not_verified") ? "Chưa xác minh" : "Đã xác minh");
            string businessCanCreateAccount = ((bodyObject["can_create_ad_account"]?.ToString() == "True") ? "Được tạo" : "Không được tạo");
            string businessPrimaryPage = bodyObject["primary_page"]?["name"]?.ToString();
            string businessTimeZoneId = bodyObject["timezone_id"]?.ToString();
            JArray businessUsers = bodyObject["business_users"]?["data"]?.ToObject<JArray>();
            string businessCreatedTime = Convert.ToDateTime(bodyObject["created_time"]?.ToString()).ToString("dd/MM/yyyy");
            string businessStatus = ((bodyObject["allow_page_management_in_www"]?.ToString() == "True") ? "BM Live" : "BM Die");
            int ownedAccountCount = int.Parse(bodyObject["owned_ad_accounts"]?["summary"]?["total_count"].ToString()) + int.Parse(bodyObject["client_ad_accounts"]?["summary"]?["total_count"].ToString());
            List<BusinessUserDto> businessUsersDto = new List<BusinessUserDto>();
            if (businessUsers != null)
            {
                foreach (JToken businessAdminObject in businessUsers)
                {
                    businessUsersDto.Add(new BusinessUserDto
                    {
                        UserId = businessAdminObject["id"]?.ToString(),
                        UserName = businessAdminObject["name"]?.ToString()
                    });
                }
            }
            BusinessInfomationDto businessDto = new BusinessInfomationDto
            {
                BusinessId = businessId,
                BusinessName = businessName,
                BusinessType = businessType,
                BusinessVerified = businessVerifyStatus,
                CanCreateAccount = businessCanCreateAccount,
                PrimaryPage = businessPrimaryPage,
                TimeZoneId = businessTimeZoneId,
                UserCount = businessUsersDto.Count.ToString(),
                CreateTime = businessCreatedTime,
                StatusBusiness = businessStatus,
                CountOwnerAdAccount = ownedAccountCount
            };
            return new BusinessManagermentDto
            {
                BusinessInfo = businessDto,
                BusinessUsers = businessUsersDto
            };
        }
        catch
        {
        }
        return null;
    }

    public async Task<List<AdAccountDto>> BatchCheckMultiAdAccount(List<string> adAccountIds)
    {
        List<AdAccountDto> adAccountDtos = new List<AdAccountDto>();
        try
        {
            if (string.IsNullOrEmpty(_account.Token))
            {
                (string, string) tokens = await GetTokenAsync();
                _account.DTSGToken = tokens.Item1;
                _account.Token = tokens.Item2;
            }
            List<List<string>> batches = new List<List<string>>();
            for (int i = 0; i < adAccountIds.Count; i += 5)
            {
                batches.Add(adAccountIds.Skip(i).Take(5).ToList());
            }
            List<Task<List<AdAccountDto>>> tasks = new List<Task<List<AdAccountDto>>>();
            foreach (List<string> batchIds in batches)
            {
                tasks.Add(ProcessBatchAsync(batchIds));
            }
            List<AdAccountDto>[] array = await Task.WhenAll(tasks);
            foreach (List<AdAccountDto> batchResult in array)
            {
                adAccountDtos.AddRange(batchResult);
            }
        }
        catch
        {
        }
        return adAccountDtos;
    }

    public async Task<List<AdAccountDto>> ProcessBatchAsync(List<string> adAccountIds)
    {
        List<AdAccountDto> adAccountDtos = new List<AdAccountDto>();
        List<object> batch = new List<object>();
        foreach (string adAccountId in adAccountIds)
        {
            string relativeUri = "act_" + adAccountId + "?fields=agencies,timezone_offset_hours_utc,spend_cap,business_country_code,funding_source_details,business,owner_business,name,account_id,disable_reason,account_status,currency,min_billing_threshold,adtrust_dsl,balance,amount_spent,account_currency_ratio_to_usd,all_payment_methods{pm_credit_card{display_string,exp_month,exp_year,is_verified}},created_time,timezone_name,userpermissions,owner,users{id,role}";
            batch.Add(new
            {
                method = "GET",
                relative_url = relativeUri
            });
        }
        Dictionary<string, string> formData = new Dictionary<string, string>
        {
            { "access_token", _account.Token },
            { "include_headers", "false" },
            {
                "batch",
                JsonConvert.SerializeObject(batch)
            }
        };
        HttpResponseMessage response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)formData), client: client, url: "https://graph.facebook.com/v15.0/", account: _account);
        response.EnsureSuccessStatusCode();
        JArray adAccountDataObjects = JArray.Parse(await response.Content.ReadAsStringAsync());
        foreach (JObject adAccountJArrObject in adAccountDataObjects)
        {
            try
            {
                if (adAccountJArrObject["code"].ToString() == "400")
                {
                    continue;
                }
                JObject bodyObject = JObject.Parse(adAccountJArrObject["body"].ToString());
                string users = string.Join(Environment.NewLine, (from x in bodyObject["userpermissions"]?["data"]?.Where((JToken x) => x["user"]?["id"] != null)
                                                                 select x["user"]["id"].ToString()) ?? Enumerable.Empty<string>());
                string partnerIds = string.Join(Environment.NewLine, (from x in bodyObject["agencies"]?["data"]?.Where((JToken x) => x["id"] != null)
                                                                      select x["id"].ToString()) ?? Enumerable.Empty<string>());
                AdAccountDto adAccountDto = new AdAccountDto
                {
                    AccountId = bodyObject["id"].ToString().Replace("act_", ""),
                    AccountName = bodyObject["name"]?.ToString(),
                    Currency = bodyObject["currency"]?.ToString(),
                    SpendCap = ((!string.IsNullOrEmpty(bodyObject["spend_cap"]?.ToString())) ? (double.Parse(bodyObject["spend_cap"].ToString()) / 100.0).ToString() : ""),
                    AccountLimit = ((bodyObject["adtrust_dsl"]?.ToString() == "-1") ? "Nolimit" : bodyObject["adtrust_dsl"]?.ToString()),
                    AccountThreshold = bodyObject["min_billing_threshold"]?["amount"]?.ToString(),
                    AccountBalance = ((!string.IsNullOrEmpty(bodyObject["balance"]?.ToString())) ? (double.Parse(bodyObject["balance"]?.ToString()) / 100.0).ToString() : ""),
                    AccountSpent = ((!string.IsNullOrEmpty(bodyObject["amount_spent"]?.ToString())) ? (double.Parse(bodyObject["amount_spent"]?.ToString()) / 100.0).ToString() : ""),
                    PaymentMethod = (string.IsNullOrEmpty(bodyObject["funding_source_details"]?["display_string"]?.ToString()) ? "N/A" : bodyObject["funding_source_details"]["display_string"].ToString()),
                    TimeZone = bodyObject["timezone_name"]?.ToString() + "/" + bodyObject["timezone_offset_hours_utc"],
                    TypeAdAccount = (string.IsNullOrEmpty(bodyObject["business"]?.ToString()) ? "Cá nhân" : "Business"),
                    CreatedTime = (Common.IsDateTimeOffset(bodyObject["created_time"].ToString()) ? Convert.ToDateTime(bodyObject["created_time"].ToString()).ToString("dd/MM/yyyy") : DateTimeOffset.FromUnixTimeSeconds(long.Parse(bodyObject["created_time"].ToString())).DateTime.ToString("dd/MM/yyyy")),
                    Owner = bodyObject["owner"]?.ToString(),
                    BusinessCountryCode = bodyObject["business_country_code"]?.ToString(),
                    BusinessId = adAccountJArrObject["business"]?["id"]?.ToString(),
                    AdAccountStatus = ((bodyObject["account_status"]?.ToString() == "1") ? "Live" : ((bodyObject["account_status"]?.ToString() == "2") ? "Vô hiệu hóa" : ((bodyObject["account_status"]?.ToString() == "3") ? "Cần thanh toán" : ((bodyObject["account_status"]?.ToString() == "101") ? "Đóng" : ((bodyObject["account_status"]?.ToString() == "7") ? "PENDING_RISK_REVIEW" : ((bodyObject["account_status"]?.ToString() == "8") ? "PENDING_SETTLEMENT" : ((bodyObject["account_status"]?.ToString() == "9") ? "IN_GRACE_PERIOD" : ((bodyObject["account_status"]?.ToString() == "100") ? "PENDING_CLOSURE" : ((bodyObject["account_status"]?.ToString() == "201") ? "ANY_ACTIVE" : ((bodyObject["account_status"]?.ToString() == "202") ? "ANY_CLOSED" : "")))))))))),
                    Users = users,
                    Partners = partnerIds
                };
                if (adAccountDto.TypeAdAccount == "Business")
                {
                    string users2 = string.Join(Environment.NewLine, (from x in bodyObject["users"]?["data"]?.Where((JToken x) => x["id"] != null)
                                                                      select x["id"].ToString()) ?? Enumerable.Empty<string>());
                    adAccountDto.Users = users2;
                }
                adAccountDtos.Add(adAccountDto);
            }
            catch
            {
            }
        }
        return adAccountDtos;
    }

    public async Task<bool> OutBusinessManage(string businessId)
    {
        if (string.IsNullOrEmpty(_account.Token))
        {
            (_account.DTSGToken, _account.Token) = await GetTokenAsync();
        }
        Dictionary<string, string> payloadDict = new Dictionary<string, string>
        {
            {
                "_reqName",
                "path:/" + _account.Uid + "/businesses"
            },
            { "business", businessId },
            {
                "endpoint",
                _account.Uid + "/businesses"
            },
            { "locale", "en_GB" },
            { "method", "delete" },
            { "pretty", "0" },
            { "suppress_http_code", "1" },
            { "version", "17.0" },
            { "userID", _account.Uid }
        };
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v17.0/" + _account.Uid + "/businesses?access_token=" + _account.Token);
        ((HttpHeaders)requestMessage.Headers).Add("Referer", "https://business.facebook.com/latest/settings/business_info?business_id=" + businessId);
        requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadDict);
        HttpResponseMessage response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        if (responseStr.Contains("{\"success\":true}"))
        {
            return true;
        }
        if (responseStr.Contains("code\":25"))
        {
            string businessUserId = await GetBusinessUserId(businessId);
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "jazoest", "25248" },
                { "fb_dtsg", _account.DTSGToken },
                { "__a", "1" },
                { "dpr", "1" },
                { "doc_id", "23932916982960697" },
                {
                    "variables",
                    "{\"businessID\":\"" + businessId + "\",\"businessUserID\":\"" + businessUserId + "\",\"surfaceParams\":null}"
                },
                { "lsd", "KnlXUiBADr-aG7YJYJ0_3y" }
            };
            response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=6798&_triggerFlowletID=6793&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("removed_business_user_id\":\"" + businessUserId + "\""))
            {
                return true;
            }
        }
        return false;
    }

    public async Task<string> GetBusinessUserId(string businessId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "jazoest", "25248" },
            { "fb_dtsg", _account.DTSGToken },
            { "__a", "1" },
            { "dpr", "1" },
            { "doc_id", "29684897197822158" },
            {
                "variables",
                "{\"globalScopeID\":\"" + businessId + "\",\"localScopeID\":null}"
            },
            { "lsd", "KnlXUiBADr-aG7YJYJ0_3y" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=6798&_triggerFlowletID=6793&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        return JObject.Parse(await response.Content.ReadAsStringAsync())["data"]?["viewer"]?["bizkit_scoping"]?["valid_local_scope"]?["null_state_global_scope"]?["global_scope"]?["business_user"]["id"].ToString();
    }

    public async Task<bool> OutAdAccount(string adAccountId)
    {
        HttpResponseMessage response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
        {
            { "jazoest", "25248" },
            { "fb_dtsg", _account.DTSGToken },
            { "__a", "1" },
            { "dpr", "1" },
            { "lsd", "KnlXUiBADr-aG7YJYJ0_3y" }
        }), client: client, url: "https://adsmanager.facebook.com/ads/manage/settings/remove_user/?user_id=" + _account.Uid + "&act=" + adAccountId + "&is_new_account_settings=1", account: _account);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("/ads\\/manage\\/accounts\\/"))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> RemoveAdAccount(string adAccountId, string businessId)
    {
        if (string.IsNullOrEmpty(_account.DTSGToken))
        {
            (_account.DTSGToken, _account.Token) = await GetTokenAsync();
        }
        HttpResponseMessage response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
        {
            { "jazoest", "25248" },
            { "fb_dtsg", _account.DTSGToken },
            { "__a", "1" },
            { "dpr", "1" },
            { "lsd", "KnlXUiBADr-aG7YJYJ0_3y" }
        }), client: client, url: "https://business.facebook.com/business/objects/remove/connections/?business_id=" + businessId + "&from_id=" + businessId + "&from_asset_type=brand&to_id=" + adAccountId + "&to_asset_type=ad-account&_callFlowletID=0&_triggerFlowletID=7963&qpl_active_e2e_trace_ids=", account: _account);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("success\":true"))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> AddAdAccountIntoBM2(string adAccountId, string businessId)
    {
        if (string.IsNullOrEmpty(_account.DTSGToken))
        {
            (_account.DTSGToken, _account.Token) = await GetTokenAsync();
        }
        HttpResponseMessage response = await HttpClientCommon.PostWithRetry(content: (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "fb_dtsg", _account.DTSGToken },
            { "__a", "1" },
            { "jazoest", "25425" },
            { "lsd", "Ag2WcUbeWHTbItjPdnEBQp" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "BizKitSettingsRequestAdAccountAccessMutation" },
            {
                "variables",
                "{\"input\":{\"client_mutation_id\":\"3\",\"actor_id\":\"" + _account.Uid + "\",\"ad_account_id\":\"" + adAccountId + "\",\"permitted_roles\":[\"864195700451909\",\"151821535410699\",\"610690166001223\",\"186595505260379\"],\"permitted_tasks\":[],\"requesting_business_id\":\"" + businessId + "\"}}"
            },
            { "locale", "en_GB" },
            { "server_timestamps", "true" },
            { "doc_id", "7293074874139866" }
        }), client: client, url: "https://business.facebook.com/api/graphql/", account: _account);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("\"access_status\":\"CONFIRMED\""))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> ChangeInfoTKQC(AdAccountInformationDto adAccountInfor, string adAccountId)
    {
        string nextUrl = "https://graph.facebook.com/v7.0/graphql?access_token=" + _account.Token + "&variables={\"input\":{\"billable_account_payment_legacy_account_id\":\"" + adAccountId + "\",\"currency\":\"" + (adAccountInfor.Currency ?? "null") + "\",\"logging_data\":{\"logging_counter\":7,\"logging_id\":\"1948616665\"},\"tax\":{\"business_address\":{\"city\":\"Vinicius Jr\",\"country_code\":\"" + adAccountInfor.CountryCode + "\",\"state\":\"AS\",\"street1\":\"Vinicius\",\"street2\":\"Kane Gray\",\"zip\":\"08386\"},\"business_name\":\"TQI Software\",\"is_personal_use\":false},\"timezone\":\"" + (adAccountInfor.TimeZone ?? "null") + "\",\"actor_id\":\"" + _account.Uid + "\",\"client_mutation_id\":\"4\"}}&doc_id=9779074012164826&method=post";
        nextUrl = nextUrl.Replace("\"null\"", "null");
        HttpResponseMessage response = await client.GetAsync(nextUrl);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("billable_account_tax_info"))
        {
            return true;
        }
        return false;
    }

    public async Task<(bool, int, int)> RemoveAllQTV(string adAccountId)
    {
        try
        {
            string url = "https://graph.facebook.com/v15.0/act_" + adAccountId + "?fields=agencies,users{id,role},business&summary=true&access_token=" + _account.Token;
            new Dictionary<string, string>
            {
                { "__a", "1" },
                { "fb_dtsg", _account.DTSGToken }
            };
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return (false, 0, 0);
            }
            JObject adAccountObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            JArray dataArray = adAccountObject["users"]?["data"] as JArray;
            int success = 0;
            int error = 0;
            if (dataArray != null)
            {
                foreach (JToken data in dataArray)
                {
                    string userId = data["id"].ToString();
                    response = await client.GetAsync("https://adsmanager-graph.facebook.com/v16.0/act_" + adAccountId + "/users/" + userId + "?method=DELETE&access_token=" + _account.Token + "&suppress_http_code=1&locale=en_US");
                    if (!response.IsSuccessStatusCode)
                    {
                        return (false, success, error);
                    }
                    if ((await response.Content.ReadAsStringAsync()).Contains("error"))
                    {
                        error++;
                    }
                    else
                    {
                        success++;
                    }
                }
            }
            if (!(adAccountObject["agencies"]?["data"] is JArray businessArr))
            {
                return (true, success, error);
            }
            foreach (JToken business in businessArr)
            {
                string businessId = business["id"].ToString();
                Dictionary<string, string> payloadRemove = new Dictionary<string, string>
                {
                    { "jazoest", "25526" },
                    { "__a", "1" },
                    { "fb_dtsg", _account.DTSGToken },
                    { "lsd", "tgNFFi_HEcq_y7_hvS3v3q" },
                    { "__aaid", adAccountId },
                    { "__user", _account.Uid }
                };
                response = await client.PostAsync("https://adsmanager.facebook.com/ads/manage/settings/remove_agency/?agency_id=" + businessId + "&act=" + adAccountId + "&_callFlowletID=0&_triggerFlowletID=4342", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadRemove));
                if (!response.IsSuccessStatusCode)
                {
                    error++;
                    continue;
                }
                MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
                string contentType2 = ((contentType != null) ? contentType.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                if ((await response.Content.ReadAsStringAsync()).Contains("error"))
                {
                    error++;
                }
                else
                {
                    success++;
                }
            }
        }
        catch
        {
        }
        return (false, 0, 0);
    }

    public async Task<(bool, int, int)> RemoveAllQTVByIG(string adAccountId)
    {
        try
        {
            string url = "https://graph.facebook.com/v15.0/act_" + adAccountId + "?fields=agencies,users{id,role},business&summary=true&access_token=" + _account.Token;
            string assetId = await GetAsset(adAccountId);
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return (false, 0, 0);
            }
            JObject dataAdAccountObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            string businessIdOwner = dataAdAccountObject["business"]["id"].ToString();
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "__a", "1" },
                { "fb_dtsg", _account.DTSGToken },
                {
                    "variables",
                    "{\"assetID\":\"" + assetId + "\",\"businessID\":\"" + businessIdOwner + "\",\"includeSystemUsers\":true,\"orderBy\":null}"
                },
                { "doc_id", "24131168866557837" }
            };
            response = await client.PostAsync("https://business.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            JObject dataBusinessAssetObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            JArray dataArray = dataBusinessAssetObject["data"]?["business"]["business_users_and_invitations"]["edges"] as JArray;
            int success = 0;
            int error = 0;
            if (dataArray != null)
            {
                foreach (JToken data in dataArray)
                {
                    string type = data["node"]["__isBusinessScopedUserOrRequest"].ToString();
                    if (!(type != "BusinessUser"))
                    {
                        string userId = data["node"]["id"].ToString();
                        payload = new Dictionary<string, string>
                        {
                            { "__a", "1" },
                            { "fb_dtsg", _account.DTSGToken },
                            { "jazoest", "25582" },
                            { "lsd", "u6HKzNGxflS9RgNzBl0hGO" },
                            {
                                "variables",
                                "{\"businessID\":\"" + businessIdOwner + "\",\"userID\":\"" + userId + "\",\"assetID\":\"" + assetId + "\",\"assetTypes\":[\"AD_ACCOUNT\"],\"isDirectAdmin\":false}"
                            },
                            { "doc_id", "25881811291409612" }
                        };
                        response = await client.PostAsync("https://business.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
                        if (!response.IsSuccessStatusCode)
                        {
                            return (false, success, error);
                        }
                        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
                        contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
                        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                        {
                            response.Content.Headers.ContentType.CharSet = "utf-8";
                        }
                        if ((await response.Content.ReadAsStringAsync()).Contains("error"))
                        {
                            error++;
                        }
                        else
                        {
                            success++;
                        }
                    }
                }
            }
            if (!(dataAdAccountObject["agencies"]?["data"] is JArray businessArr))
            {
                return (true, success, error);
            }
            foreach (JToken business in businessArr)
            {
                string businessId = business["id"].ToString();
                Dictionary<string, string> payloadRemove = new Dictionary<string, string>
                {
                    { "jazoest", "25526" },
                    { "__a", "1" },
                    { "fb_dtsg", _account.DTSGToken },
                    { "lsd", "tgNFFi_HEcq_y7_hvS3v3q" },
                    { "__aaid", adAccountId },
                    { "__user", _account.Uid }
                };
                response = await client.PostAsync("https://adsmanager.facebook.com/ads/manage/settings/remove_agency/?agency_id=" + businessId + "&act=" + adAccountId + "&_callFlowletID=0&_triggerFlowletID=4342", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadRemove));
                if (!response.IsSuccessStatusCode)
                {
                    error++;
                    continue;
                }
                MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
                contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                if ((await response.Content.ReadAsStringAsync()).Contains("error"))
                {
                    error++;
                }
                else
                {
                    success++;
                }
            }
        }
        catch
        {
        }
        return (false, 0, 0);
    }

    public async Task<StatusShareBM> ShareBMRequest(string brandId, string email, bool antispam = true)
    {
        try
        {
            if (antispam)
            {
                string[] emailRaw = email.Split('@');
                email = emailRaw[0] + "+tqi" + Common.CreateRandomNumber(7) + "@" + emailRaw[1];
            }
            if (string.IsNullOrEmpty(_account.Token))
            {
                (_account.DTSGToken, _account.Token) = await GetTokenAsync();
            }
            HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/graphql?method=post&fb_api_caller_class=RelayModern&fb_api_req_friendly_name=BizKitSettingsInvitePeopleModalMutation&variables={\"input\":{\"client_mutation_id\":\"5\",\"actor_id\":\"" + _account.Uid + "\",\"business_id\":\"" + brandId + "\",\"business_emails\":[\"" + email + "\"],\"business_account_task_ids\":[\"926381894526285\",\"603931664885191\",\"1327662214465567\",\"862159105082613\",\"6161001899617846786\",\"1633404653754086\",\"967306614466178\",\"2848818871965443\",\"245181923290198\",\"388517145453246\",\"768085000593466\",\"416103972652535\"],\"invite_origin_surface\":\"MBS_INVITE_USER_FLOW\",\"assets\":[],\"expiry_time\":0,\"client_timezone_id\":\"Asia/Jakarta\"}}&server_timestamps=true&doc_id=9562153673860850&access_token=" + _account.Token);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("checkpoint_url"))
            {
                return StatusShareBM.Checkpoint282;
            }
            if (!response.IsSuccessStatusCode)
            {
                return StatusShareBM.Error;
            }
            if (responseStr.Contains("code\":2859031"))
            {
                return StatusShareBM.Error;
            }
            if (responseStr.Contains("id"))
            {
                return StatusShareBM.Success;
            }
        }
        catch
        {
        }
        return StatusShareBM.Failed;
    }

    public async Task<(StatusCreateBM, string)> CreateAdAccountInBMVer2(string businessId, string name, string currency, string timeZone, string endAdvertiser = "")
    {
        try
        {
            endAdvertiser = (string.IsNullOrEmpty(endAdvertiser) ? businessId : endAdvertiser);
            HttpResponseMessage response = await client.GetAsync("https://business.facebook.com/latest/settings/ad_accounts?business_id=" + businessId);
            if (!response.IsSuccessStatusCode)
            {
                return (StatusCreateBM.Error, "");
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            _account.LSDToken = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\\\"").Groups[1].Value;
            Dictionary<string, string> formData = new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "__aaid", "0" },
                { "__bid", businessId },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "__req", "1b" },
                { "__hs", "20322.HYP:bizweb_comet_pkg.2.1...0" },
                { "dpr", "1" },
                { "__ccg", "EXCELLENT" },
                { "__rev", "1026212048" },
                { "__s", "7nafg7:r2gp3k:dkc6jv" },
                { "__hsi", "7541412880539086154" },
                { "__dyn", "7xeUmxa2C6onwn8K2Wmh0MBwCwpUnwgU29zEdF8ixy361twYwJw5hz8hw9-0r-qbwgE7Ro4C0RUb87C2m3K2y11wBz8188O12ypU5-2a1Owv89k2C1FwnE4K5E2sx2ewyx6i2GU8U-UbE7i4UaEW2G261fwwwJK1qxa1ozEjU4Wdwoo4S5ayocE3BwMw_CyKbwzwea0Lo6-3u36i2G0z8co9U4S7E6B0gEjz8158uwm85K0E888882qwb20NU" },
                { "__csr", "g47k29MF7PigJEApOTRv9MGV29OnOHORkBAFveGL8YKylTcCOtkgDB9lnPdHRnmWiQW8y9kBinHYzGQFqmpRAirqHtuyGAYFQOHXnHuBigJqnhuVAEKnZlGRh4GjmX-mGGi8XhyfA_Z-FbzQEF9qgChlJGSap-hx7XgKij9F7VKAUTzURCJ5QibADlfUSrgjyeFECXpqZCiKV8SutdatkayppuqLF2RKUSEnggxHigKmQWyq-Hy4UCUG8yJ2uqUqzoipZ7zbxai4rhUy9DUBa4UGKq9Umyumt3pohyoiyZzFpGDCggUhG2yieyEOGx10BF2awIzofUbEaamdykEaqCwwxe2udwQxi5U-3jwDx2bK8w9S69E5uu3mfxO12wyw2Eo3py8G0MU3hISRmPK6Q04hU1fpEvxirw3UbwfS1fhtOyoF1i5BADws8C5E2hgiwF2oEAwdd0ooK8wnMP0wEF8DxpaNEzgS0mR1wAwS2KApiDIQU660y0nglHeF952wbA6k4UaMbw0N61-K08MK1Qw278dem0NA4S4EaQ3i_weK49E7ebDg0sTAw1E209fK14yE0e5iwaq0s10Qw1cC1gy83Vyoy220XEhw1xsloy2N02sU0urBo3Owdu5pao1LEygw-0czg2Qg0ZOpCyoiw2480BME0Zy8K04aEx2VQ36QYw138y0wQ6UfE08jQsw0NC0kmgE0ey8s80P89Au9w3LU4Gm0um0Q8sKmbCo0PB2m2m9wUw823C09Sw_w1z50zxG0xJwDyk0R8so8E9o4C0e9w3J4" },
                { "__hsdp", "guMR5M8X88jBcPO5NQbGv5cGkiiAQyn4R4O9mwryNRmt4BhFXhQeAzdkVURQj42UDtBhp21iUfQ5GNM4aWoKq3e3icwyxp7xwwpjgDc2iagjHwkaCDx62C3SFqg5rxPUGc-8KoHK8CzA5E1k81vU0wK02e23i0-E0tbw8y0a5w20o0Dm0xo0FO058U" },
                { "__hblp", "0ZBz8W0VEbEuwgE8E-5UtDAwNBxm0EE9E4m1hwh6axe19WUhwgpeu4oaofqBwmK7fyEPoyVy4J7Cwaacxa4V989876782Cwmo5K3614wyyEswvo6bU4i2maCwdS1byovwhFQ3qbGU47K1dw8C1HxO3-3qii2a15wCypUpwAwk8y0yEmDyE2iwaa0FEryo27xu2e3i1ww9C0t26EgAwkoiCxK0EHxi361gwFAwFxC5E8UuwkoK0Wo8Uf8iwem4U622u0hai18x618xq2W6U6u0FoeawgGwBCxeewLwBgtgG8wDwme5EeU6i1cwb6i2a4u5VQ58swOwwU4O2m2mex-2e1gxOaxK0Co5W2G482yBU3AxWawrUd8qwZzob8owfu3G1PwQwio4K79KawJwWg4u4UlwpU26xm" },
                { "__sjsp", "guMR5M8X88jBcPO5NQbGv5cGkiiAQyn4QPO9mI6EItlDh9kquQt3F8Pleudt4N0K9TpkmgwkK3Z1qIs12KCbCwPwQz88EmhUo86kQ9P0AyA4WU52FFUhwFwZGmA1mUs-azfybCaXy9EV1q0l20n-08bw" },
                { "__comet_req", "11" },
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25461" },
                { "lsd", _account.LSDToken },
                { "__spin_r", "1026212048" },
                { "__spin_b", "trunk" },
                {
                    "__spin_t",
                    $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}"
                },
                { "__jssesw", "1" },
                { "__crn", "comet.bizweb.BusinessCometBizSuiteSettingsAdAccountsRoute" },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "BizKitSettingsCreateAdAccountUsageStepQuery" },
                {
                    "variables",
                    "{\"businessID\":\"" + businessId + "\"}"
                },
                { "server_timestamps", "true" },
                { "doc_id", "30132031866444376" }
            };
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://business.facebook.com/api/graphql/?_callFlowletID=7571&_triggerFlowletID=7566&qpl_active_e2e_trace_ids=");
            ((HttpHeaders)httpRequestMessage.Headers).Add("x-fb-lsd", _account.LSDToken);
            httpRequestMessage.Headers.Referrer = new Uri("https://business.facebook.com/latest/settings/ad_accounts?business_id=" + businessId);
            httpRequestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)formData);
            await client.SendAsync(httpRequestMessage);
            formData = new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "__aaid", "0" },
                { "__bid", businessId },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "__req", "1b" },
                { "__hs", "20322.HYP:bizweb_comet_pkg.2.1...0" },
                { "dpr", "1" },
                { "__ccg", "EXCELLENT" },
                { "__rev", "1026212048" },
                { "__s", "7nafg7:r2gp3k:dkc6jv" },
                { "__hsi", "7541412880539086154" },
                { "__dyn", "7xeUmxa2C6onwn8K2Wmh0MBwCwpUnwgU29zEdF8ixy361twYwJw5hz8hw9-0r-qbwgE7Ro4C0RUb87C2m3K2y11wBz8188O12ypU5-2a1Owv89k2C1FwnE4K5E2sx2ewyx6i2GU8U-UbE7i4UaEW2G261fwwwJK1qxa1ozEjU4Wdwoo4S5ayocE3BwMw_CyKbwzwea0Lo6-3u36i2G0z8co9U4S7E6B0gEjz8158uwm85K0E888882qwb20NU" },
                { "__csr", "g47k29MF7PigJEApOTRv9MGV29OnOHORkBAFveGL8YKylTcCOtkgDB9lnPdHRnmWiQW8y9kBinHYzGQFqmpRAirqHtuyGAYFQOHXnHuBigJqnhuVAEKnZlGRh4GjmX-mGGi8XhyfA_Z-FbzQEF9qgChlJGSap-hx7XgKij9F7VKAUTzURCJ5QibADlfUSrgjyeFECXpqZCiKV8SutdatkayppuqLF2RKUSEnggxHigKmQWyq-Hy4UCUG8yJ2uqUqzoipZ7zbxai4rhUy9DUBa4UGKq9Umyumt3pohyoiyZzFpGDCggUhG2yieyEOGx10BF2awIzofUbEaamdykEaqCwwxe2udwQxi5U-3jwDx2bK8w9S69E5uu3mfxO12wyw2Eo3py8G0MU3hISRmPK6Q04hU1fpEvxirw3UbwfS1fhtOyoF1i5BADws8C5E2hgiwF2oEAwdd0ooK8wnMP0wEF8DxpaNEzgS0mR1wAwS2KApiDIQU660y0nglHeF952wbA6k4UaMbw0N61-K08MK1Qw278dem0NA4S4EaQ3i_weK49E7ebDg0sTAw1E209fK14yE0e5iwaq0s10Qw1cC1gy83Vyoy220XEhw1xsloy2N02sU0urBo3Owdu5pao1LEygw-0czg2Qg0ZOpCyoiw2480BME0Zy8K04aEx2VQ36QYw138y0wQ6UfE08jQsw0NC0kmgE0ey8s80P89Au9w3LU4Gm0um0Q8sKmbCo0PB2m2m9wUw823C09Sw_w1z50zxG0xJwDyk0R8so8E9o4C0e9w3J4" },
                { "__hsdp", "guMR5M8X88jBcPO5NQbGv5cGkiiAQyn4R4O9mwryNRmt4BhFXhQeAzdkVURQj42UDtBhp21iUfQ5GNM4aWoKq3e3icwyxp7xwwpjgDc2iagjHwkaCDx62C3SFqg5rxPUGc-8KoHK8CzA5E1k81vU0wK02e23i0-E0tbw8y0a5w20o0Dm0xo0FO058U" },
                { "__hblp", "0ZBz8W0VEbEuwgE8E-5UtDAwNBxm0EE9E4m1hwh6axe19WUhwgpeu4oaofqBwmK7fyEPoyVy4J7Cwaacxa4V989876782Cwmo5K3614wyyEswvo6bU4i2maCwdS1byovwhFQ3qbGU47K1dw8C1HxO3-3qii2a15wCypUpwAwk8y0yEmDyE2iwaa0FEryo27xu2e3i1ww9C0t26EgAwkoiCxK0EHxi361gwFAwFxC5E8UuwkoK0Wo8Uf8iwem4U622u0hai18x618xq2W6U6u0FoeawgGwBCxeewLwBgtgG8wDwme5EeU6i1cwb6i2a4u5VQ58swOwwU4O2m2mex-2e1gxOaxK0Co5W2G482yBU3AxWawrUd8qwZzob8owfu3G1PwQwio4K79KawJwWg4u4UlwpU26xm" },
                { "__sjsp", "guMR5M8X88jBcPO5NQbGv5cGkiiAQyn4QPO9mI6EItlDh9kquQt3F8Pleudt4N0K9TpkmgwkK3Z1qIs12KCbCwPwQz88EmhUo86kQ9P0AyA4WU52FFUhwFwZGmA1mUs-azfybCaXy9EV1q0l20n-08bw" },
                { "__comet_req", "11" },
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25461" },
                { "lsd", _account.LSDToken },
                { "__spin_r", "1026212048" },
                { "__spin_b", "trunk" },
                { "__spin_t", "1755872015" },
                { "__jssesw", "1" },
                { "__crn", "comet.bizweb.BusinessCometBizSuiteSettingsAdAccountsRoute" },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "BizKitSettingsCreateAdAccountMutation" },
                {
                    "variables",
                    "{\"businessID\":\"" + businessId + "\",\"adAccountName\":\"" + name + "\",\"timezoneID\":\"" + timeZone + "\",\"currency\":\"" + currency + "\",\"endAdvertiserID\":\"" + endAdvertiser + "\"}"
                },
                { "server_timestamps", "true" },
                { "doc_id", "9236789956426634" }
            };
            httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://business.facebook.com/api/graphql/?_callFlowletID=7571&_triggerFlowletID=7566&qpl_active_e2e_trace_ids=");
            ((HttpHeaders)httpRequestMessage.Headers).Add("x-fb-lsd", _account.LSDToken);
            httpRequestMessage.Headers.Referrer = new Uri("https://business.facebook.com/latest/settings/ad_accounts?business_id=" + businessId);
            httpRequestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)formData);
            response = await client.SendAsync(httpRequestMessage);
            if (!response.IsSuccessStatusCode)
            {
                MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
                contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                await response.Content.ReadAsStringAsync();
                return (StatusCreateBM.Error, "");
            }
            MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
            contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("business_object_ui_id"))
            {
                string adAccountId = JObject.Parse(responseStr)["data"]["business_settings_create_ad_account"]["id"].ToString();
                try
                {
                    formData = new Dictionary<string, string>
                    {
                        { "av", _account.Uid },
                        { "__aaid", "0" },
                        { "__bid", businessId },
                        { "__user", _account.Uid },
                        { "__a", "1" },
                        { "__req", "1b" },
                        { "__hs", "20322.HYP:bizweb_comet_pkg.2.1...0" },
                        { "dpr", "1" },
                        { "__ccg", "EXCELLENT" },
                        { "__rev", "1026212048" },
                        { "__s", "7nafg7:r2gp3k:dkc6jv" },
                        { "__hsi", "7541412880539086154" },
                        { "__dyn", "7xeUmxa2C6onwn8K2Wmh0MBwCwpUnwgU29zEdF8ixy361twYwJw5hz8hw9-0r-qbwgE7Ro4C0RUb87C2m3K2y11wBz8188O12ypU5-2a1Owv89k2C1FwnE4K5E2sx2ewyx6i2GU8U-UbE7i4UaEW2G261fwwwJK1qxa1ozEjU4Wdwoo4S5ayocE3BwMw_CyKbwzwea0Lo6-3u36i2G0z8co9U4S7E6B0gEjz8158uwm85K0E888882qwb20NU" },
                        { "__csr", "g47k29MF7PigJEApOTRv9MGV29OnOHORkBAFveGL8YKylTcCOtkgDB9lnPdHRnmWiQW8y9kBinHYzGQFqmpRAirqHtuyGAYFQOHXnHuBigJqnhuVAEKnZlGRh4GjmX-mGGi8XhyfA_Z-FbzQEF9qgChlJGSap-hx7XgKij9F7VKAUTzURCJ5QibADlfUSrgjyeFECXpqZCiKV8SutdatkayppuqLF2RKUSEnggxHigKmQWyq-Hy4UCUG8yJ2uqUqzoipZ7zbxai4rhUy9DUBa4UGKq9Umyumt3pohyoiyZzFpGDCggUhG2yieyEOGx10BF2awIzofUbEaamdykEaqCwwxe2udwQxi5U-3jwDx2bK8w9S69E5uu3mfxO12wyw2Eo3py8G0MU3hISRmPK6Q04hU1fpEvxirw3UbwfS1fhtOyoF1i5BADws8C5E2hgiwF2oEAwdd0ooK8wnMP0wEF8DxpaNEzgS0mR1wAwS2KApiDIQU660y0nglHeF952wbA6k4UaMbw0N61-K08MK1Qw278dem0NA4S4EaQ3i_weK49E7ebDg0sTAw1E209fK14yE0e5iwaq0s10Qw1cC1gy83Vyoy220XEhw1xsloy2N02sU0urBo3Owdu5pao1LEygw-0czg2Qg0ZOpCyoiw2480BME0Zy8K04aEx2VQ36QYw138y0wQ6UfE08jQsw0NC0kmgE0ey8s80P89Au9w3LU4Gm0um0Q8sKmbCo0PB2m2m9wUw823C09Sw_w1z50zxG0xJwDyk0R8so8E9o4C0e9w3J4" },
                        { "__hsdp", "guMR5M8X88jBcPO5NQbGv5cGkiiAQyn4R4O9mwryNRmt4BhFXhQeAzdkVURQj42UDtBhp21iUfQ5GNM4aWoKq3e3icwyxp7xwwpjgDc2iagjHwkaCDx62C3SFqg5rxPUGc-8KoHK8CzA5E1k81vU0wK02e23i0-E0tbw8y0a5w20o0Dm0xo0FO058U" },
                        { "__hblp", "0ZBz8W0VEbEuwgE8E-5UtDAwNBxm0EE9E4m1hwh6axe19WUhwgpeu4oaofqBwmK7fyEPoyVy4J7Cwaacxa4V989876782Cwmo5K3614wyyEswvo6bU4i2maCwdS1byovwhFQ3qbGU47K1dw8C1HxO3-3qii2a15wCypUpwAwk8y0yEmDyE2iwaa0FEryo27xu2e3i1ww9C0t26EgAwkoiCxK0EHxi361gwFAwFxC5E8UuwkoK0Wo8Uf8iwem4U622u0hai18x618xq2W6U6u0FoeawgGwBCxeewLwBgtgG8wDwme5EeU6i1cwb6i2a4u5VQ58swOwwU4O2m2mex-2e1gxOaxK0Co5W2G482yBU3AxWawrUd8qwZzob8owfu3G1PwQwio4K79KawJwWg4u4UlwpU26xm" },
                        { "__sjsp", "guMR5M8X88jBcPO5NQbGv5cGkiiAQyn4QPO9mI6EItlDh9kquQt3F8Pleudt4N0K9TpkmgwkK3Z1qIs12KCbCwPwQz88EmhUo86kQ9P0AyA4WU52FFUhwFwZGmA1mUs-azfybCaXy9EV1q0l20n-08bw" },
                        { "__comet_req", "11" },
                        { "fb_dtsg", _account.DTSGToken },
                        { "jazoest", "25461" },
                        { "lsd", _account.LSDToken },
                        { "__spin_r", "1026212048" },
                        { "__spin_b", "trunk" },
                        { "__spin_t", "1755872015" },
                        { "__jssesw", "1" },
                        { "__crn", "comet.bizweb.BusinessCometBizSuiteSettingsAdAccountsRoute" },
                        { "fb_api_caller_class", "RelayModern" },
                        { "fb_api_req_friendly_name", "BizKitSettingsBusinessAssetsListPaginationQuery" },
                        {
                            "variables",
                            "{\"assetFilters\":{\"ad_account_statuses\":[\"ACTIVE\",\"DISABLED\",\"IN_GRACE_PERIOD\",\"PENDING_CLOSURE\",\"PENDING_RISK_REVIEW\",\"PENDING_SETTLEMENT\",\"UNSETTLED\"]},\"assetTypes\":[\"AD_ACCOUNT\"],\"businessID\":\"" + businessId + "\",\"count\":14,\"cursor\":null,\"globalFilters\":null,\"orderBy\":null,\"searchTerm\":null,\"shouldCountAdmin\":false,\"id\":\"" + businessId + "\"}"
                        },
                        { "server_timestamps", "true" },
                        { "doc_id", "24040566188906095" }
                    };
                    httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://business.facebook.com/api/graphql/?_callFlowletID=7571&_triggerFlowletID=7566&qpl_active_e2e_trace_ids=");
                    ((HttpHeaders)httpRequestMessage.Headers).Add("x-fb-lsd", _account.LSDToken);
                    httpRequestMessage.Headers.Referrer = new Uri("https://business.facebook.com/latest/settings/ad_accounts?business_id=" + businessId);
                    httpRequestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)formData);
                    await client.SendAsync(httpRequestMessage);
                }
                catch
                {
                }
                return (StatusCreateBM.Success, adAccountId);
            }
            if (responseStr.Contains("code\":190"))
            {
                return (StatusCreateBM.ReLogin, "");
            }
            if (responseStr.Contains("\"api_error_code\":3979"))
            {
                return (StatusCreateBM.Block, "Đạt giới hạn TKQC");
            }
            if (responseStr.Contains("\"api_error_code\":3980"))
            {
                return (StatusCreateBM.Block, "Có TK die hoặc nợ");
            }
        }
        catch
        {
        }
        return (StatusCreateBM.Fail, "Không rõ");
    }

    public async Task<EnumStatus.CloseStatus> CloseAdAccountInBM(string adAccountId)
    {
        try
        {
            Dictionary<string, string> content = new Dictionary<string, string>
            {
                { "jazoest", "25276" },
                { "fb_dtsg", _account.DTSGToken },
                { "account_id", adAccountId },
                { "__usid", "6-Tska1fl1gmx7sf%3APskabwl10zaq7%3A0-Askabmt1xwgzsb-RV%3D6%3AF%3D" },
                { "__aaid", "0" },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "__req", "t" },
                { "__hs", "19989.BP:brands_pkg.2.0..0.0" },
                { "dpr", "1" },
                { "__ccg", "EXCELLENT" },
                { "__rev", "1016720617" },
                { "__s", "rcl785:ynzvij:4s63oy" },
                { "__hsi", "7417954994940856479" },
                { "__dyn", "7xeUmxa2C5rgydwCwRyUbFp4Unxim2q1Dxuq3mq1FxebzA3miidBxa7EiwnobES2S2q1Ex21FxG9y8Gdz8hw9-3a4EuCwQwCxq0yFE4WqbwQzobVqxN0Cmu3mbx-261UxO4UkK2y1gwBwXwEw-G2mcwuE2Bz84a9DxW10wywWjxCU5-u2C2l0Fg6y3m2y1bxq1yxJxK48GU8EhAwGK2efK7UW1dx-q4VEhwwwj84-224U-dwmEiwm8Wubwk8S2a3WcwMzUkGu3i2WE4e8wpEK4EhzUbVEHyU8U3yDwbm1LwqpbwCwiUWqU9EnxC2u1dxW6U98a85Ou0hi1TwmUaE2mw" },
                { "__csr", "" },
                { "lsd", "LV9hlWZLUYMQSnabfYCIrT" },
                { "__spin_r", "1016720617" },
                { "__spin_b", "trunk" },
                { "__spin_t", "1727127236" },
                { "__jssesw", "1" },
                { "qpl_active_flow_ids", "820461434" }
            };
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://business.facebook.com/ads/ajax/account_close/?_callFlowletID=0&_triggerFlowletID=8391");
            ((HttpHeaders)requestMessage.Headers).Add("x-fb-friendly-name", "useBillingReactivateAdAccountMutation");
            ((HttpHeaders)requestMessage.Headers).Add("x-fb-lsd", "LV9hlWZLUYMQSnabfYCIrT");
            ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-site", "same-origin");
            ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-mode", "cors");
            requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)content);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return EnumStatus.CloseStatus.Failed;
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if (!(await response.Content.ReadAsStringAsync()).Contains("error"))
            {
                return EnumStatus.CloseStatus.Success;
            }
        }
        catch
        {
        }
        return EnumStatus.CloseStatus.Failed;
    }

    public async Task<bool> ReActiveAdAccount(string adAccountId)
    {
        Dictionary<string, string> content = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__usid", "6-Tskac8l1fpr8sw%3APskac8h17c3ivj%3A0-Askabmt1xwgzsb-RV%3D6%3AF%3D" },
            { "__aaid", adAccountId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "3d" },
            { "__hs", "19989.BP:ads_manager_pkg.2.0..0.0" },
            { "dpr", "1" },
            { "__ccg", "UNKNOWN" },
            { "__rev", "1016726306" },
            { "__s", "rpgr19:ywru02:ncs5ef" },
            { "__hsi", "7417956838382605379" },
            { "__dyn", "7AgSXgWGgWEjgDBxmSudg9omoiyoK6FVpkihG5Xx2m2q3K2KmeGqKi5axeqaScCCG225pojACjyocuF98SmqnK7GzUuwDxq4EOezoK26UKbC-mdwTxOESegGbwgEmK9y8Gdz8hyUuxqt1eiUO4EgCyku4oS4EWfGUhwyg9p44889EScxyu6UGq13yHGmmUTxJe9LgbeWG9DDl0zlBwyzp8KUV2U8oK1IxO4VAcKmieyp8BlBUK2O4UOi3Kdx29wgojKbUO1Wxu4GBwkEuz478shECumbz8KiewwBK68eF9UhK1vDyojyUix92UtgKi3a6Ex0RyQcKazQ3G5EbpEtzA6Sax248GUgz98hAy8tKU-4U-UG7F8a898vCxeq4qz8gwDzElx63Si6UjzUS324UGaxa2h2ppEryrhUK5Ue8Su6Ey3maUjxy-dxiFAm9KcyoC2GZ3UC2C8ByoF1a58gx6bxa4oOE88ymqaUK2e4E42byE6-uvAxO0yVoK3Cd868g-cwNxaHjwCwXzUWrUlUym5UpU9oeUhxWUnposxx7KAfwxCwyDxm5V9UWaV-8G54VoC12yp8aUmxl4Ki2iUhjy8999eta4ozDz8G8wOJ129hocU" },
            { "__csr", "" },
            { "__comet_req", "25" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25336" },
            { "lsd", "6rcZCHasNekDQJU_hmWq6J" },
            { "__spin_r", "1016726306" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1727127665" },
            { "__jssesw", "1" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useBillingReactivateAdAccountMutation" },
            {
                "variables",
                "{\"input\":{\"billable_account_payment_legacy_account_id\":\"" + adAccountId + "\",\"logging_data\":{\"logging_counter\":9,\"logging_id\":\"2396824380\"},\"upl_logging_data\":{\"context\":\"billingaccountinfo\",\"entry_point\":\"power_editor_rhr\",\"external_flow_id\":\"\",\"target_name\":\"BillingReactivateAdAccountMutation\",\"user_session_id\":\"upl_1727127670627_bb2ffd83-fbf6-447a-978a-0112805729b5\",\"wizard_config_name\":\"REACTIVATE_AD_ACCOUNT\",\"wizard_name\":\"REACTIVATE_AD_ACCOUNT\",\"wizard_screen_name\":\"reactivate_ad_account_state_display\",\"wizard_session_id\":\"upl_wizard_1727127693443_f6b6cba9-60d3-4ec7-83fc-26f0c472e30f\",\"wizard_state_name\":\"reactivate_ad_account_state_display\"},\"actor_id\":\"" + _account.Uid + "\",\"client_mutation_id\":\"8\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9984888131552276" }
        };
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://adsmanager.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=15658");
        ((HttpHeaders)requestMessage.Headers).Add("x-fb-friendly-name", "useBillingReactivateAdAccountMutation");
        ((HttpHeaders)requestMessage.Headers).Add("x-fb-lsd", "6rcZCHasNekDQJU_hmWq6J");
        ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-site", "same-origin");
        ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-mode", "cors");
        requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)content);
        HttpResponseMessage response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("ADMARKET_ACCOUNT_STATUS_ACTIVE"))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> AssignPartner(string adAccountId, string businessId, string partnerId, int type)
    {
        if (string.IsNullOrEmpty(_account.Token))
        {
            (_account.DTSGToken, _account.Token) = await GetTokenAsync();
        }
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "__activeScenarioIDs", "[]" },
            { "__activeScenarios", "[]" },
            { "__interactionsMetadata", "[]" },
            { "_reqName", "adaccount/agencies" },
            { "_reqSrc", "BrandAgencyActions.brands" },
            { "accountId", adAccountId },
            { "acting_brand_id", businessId },
            { "business", partnerId },
            { "locale", "vi_VN" },
            { "method", "post" },
            { "permitted_tasks", "[\"ADVERTISE\",\"ANALYZE\",\"DRAFT\",\"MANAGE\"]" }
        };
        string url = "https://graph.facebook.com/v17.0/act_" + adAccountId + "/agencies?access_token=" + _account.Token;
        if (type != 0 && type == 1)
        {
            payload["permitted_tasks"] = "[\"ADVERTISE\",\"ANALYZE\",\"DRAFT\"]";
        }
        HttpResponseMessage response = await client.PostAsync(url, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        if (responseStr.Contains("\"success\": true"))
        {
            return true;
        }
        if (responseStr.Contains("\"code\":2859009"))
        {
            if (string.IsNullOrEmpty(_account.Key2FA))
            {
                return false;
            }
            string code = Common.GetCode(_account.Key2FA);
            Dictionary<string, string> payloadVerifyCode = new Dictionary<string, string>
            {
                { "approvals_code", code },
                { "save_device", "false" },
                { "hash", "" },
                { "__aaid", "0" },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "__req", "6" },
                { "__hs", "20281.BP:DEFAULT.2.0...0" },
                { "dpr", "1" },
                { "__ccg", "EXCELLENT" },
                { "__rev", "1024691666" },
                { "__s", "v69om1:ii45zd:aazj17" },
                { "__hsi", "7526098785714860887" },
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25133" },
                { "lsd", "DPyYVDMXTBviNmF7ZRS8Cs" },
                { "__spin_r", "1024691666" },
                { "__spin_b", "trunk" },
                { "__spin_t", "1752306424" },
                { "__jssesw", "1" }
            };
            response = await client.PostAsync("https://business.facebook.com/security/twofactor/reauth/enter/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadVerifyCode));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
            contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
            response = await client.PostAsync(url, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
            MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
            contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"success\":true"))
            {
                return true;
            }
        }
        else if (responseStr.Contains("error_subcode\": 2859043"))
        {
            if (string.IsNullOrEmpty(_account.Key2FA))
            {
                return false;
            }
            string code2 = Common.GetCode(_account.Key2FA);
            Dictionary<string, string> payloadVerifyCode2 = new Dictionary<string, string>
            {
                { "approvals_code", code2 },
                { "save_device", "false" },
                { "hash", "" },
                { "__aaid", "0" },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "__req", "6" },
                { "__hs", "20281.BP:DEFAULT.2.0...0" },
                { "dpr", "1" },
                { "__ccg", "EXCELLENT" },
                { "__rev", "1024691666" },
                { "__s", "v69om1:ii45zd:aazj17" },
                { "__hsi", "7526098785714860887" },
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25133" },
                { "lsd", "DPyYVDMXTBviNmF7ZRS8Cs" },
                { "__spin_r", "1024691666" },
                { "__spin_b", "trunk" },
                { "__spin_t", "1752306424" },
                { "__jssesw", "1" }
            };
            response = await client.PostAsync("https://business.facebook.com/security/twofactor/reauth/enter/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadVerifyCode2));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue contentType5 = response.Content.Headers.ContentType;
            contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
            response = await client.PostAsync(url, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
            MediaTypeHeaderValue contentType6 = response.Content.Headers.ContentType;
            contentType2 = ((contentType6 != null) ? contentType6.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"success\":true"))
            {
                return true;
            }
        }
        return false;
    }

    public async Task<bool> AssignPermissionUserIntoAdAccount(string adAccountId, string businessId, string userId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25437" },
            { "lsd", "z2Os7htu8cySYSZlPhDCnx" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "BusinessToolsAssignUserToAssetModalMutation" },
            { "doc_id", "8519783498080407" },
            {
                "variables",
                "{\"businessID\":\"" + businessId + "\",\"assetID\":\"" + adAccountId + "\",\"userIDs\":[\"" + userId + "\"],\"taskIDs\":[\"864195700451909\",\"151821535410699\",\"610690166001223\",\"186595505260379\"]}"
            }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("result_type\":\"SUCCESS\""))
        {
            return true;
        }
        return false;
    }

    public async Task<string> CheckPTTT(string adAccountId)
    {
        try
        {
            string nextUrl = "https://graph.facebook.com/graphql?access_token=" + _account.Token + "&variables={\"paymentAccountID\":\"" + adAccountId + "\"}&doc_id=6975887429148122&method=post";
            HttpResponseMessage response = await client.GetAsync(nextUrl);
            if (!response.IsSuccessStatusCode)
            {
                return "Error";
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            JArray paymentJArr = JObject.Parse(responseStr)["data"]["billable_account_by_payment_account"]["billing_payment_account"]["billing_payment_methods"].ToObject<JArray>();
            string paymentStr = string.Empty;
            foreach (JToken payment in paymentJArr)
            {
                if (payment["credential"]["__typename"].ToString().Equals("ExternalCreditCard"))
                {
                    string needVeri = ((payment["credential"]["needs_verification"].ToString() == "True") ? "Need" : "");
                    paymentStr = ((!payment["is_primary"].ToObject<bool>()) ? (paymentStr + "P_" + payment["credential"]["card_association"].ToString() + " *" + payment["credential"]["last_four_digits"].ToString() + "-" + needVeri + Environment.NewLine) : (paymentStr + "C_" + payment["credential"]["card_association"].ToString() + " *" + payment["credential"]["last_four_digits"].ToString() + "-" + needVeri + Environment.NewLine));
                }
            }
            string creditCard = Regex.Match(responseStr, "\"card_association_name\":\"(.*?)\",").Groups[1].Value;
            if (string.IsNullOrEmpty(creditCard))
            {
                return "Không có thẻ";
            }
            if (responseStr.Contains("is_reauth_restricted\":true,"))
            {
                return "Hold";
            }
            if (responseStr.Contains("UNVERIFIED_OR_PENDING") || responseStr.Contains("PENDING_VERIFICATION"))
            {
                return "Xác minh thẻ";
            }
            return "Bình thường";
        }
        catch
        {
            return "Exception";
        }
    }

    public async Task<bool> CreateRule(string adAccountId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "__aaid", adAccountId },
            { "__activeScenarioIDs", "[]" },
            { "__activeScenarios", "[]" },
            { "__interactionsMetadata", "[]" },
            { "_reqName", "adaccount/adrules_library" },
            { "_reqSrc", "AdsRuleDataManager" },
            { "ads_manager_write_regions", "true" },
            { "evaluation_spec", "{\"evaluation_type\":\"SCHEDULE\",\"filters\":[{\"field\":\"spent\",\"operator\":\"GREATER_THAN\",\"value\":\"1\"},{\"field\":\"entity_type\",\"value\":\"AD\",\"operator\":\"EQUAL\"},{\"field\":\"time_preset\",\"value\":\"MAXIMUM\",\"operator\":\"EQUAL\"}]}" },
            {
                "execution_spec",
                "{\"execution_type\":\"PAUSE\",\"execution_options\":[{\"field\":\"user_ids\",\"value\":[\"" + _account.Uid + "\"],\"operator\":\"EQUAL\"},{\"field\":\"alert_preferences\",\"value\":{\"instant\":{\"trigger\":\"CHANGE\"}},\"operator\":\"EQUAL\"}]}"
            },
            { "include_headers", "false" },
            { "locale", "en_US" },
            { "method", "post" },
            { "name", "tqi" },
            { "schedule_spec", "{\"schedule_type\":\"SEMI_HOURLY\"}" },
            { "status", "ENABLED" },
            { "ui_creation_source", "pe_toolbar_create_rule_dropdown" }
        };
        string url = "https://adsmanager-graph.facebook.com/v22.0/act_" + adAccountId + "/adrules_library?_reqName=adaccount%2Fadrules_library&access_token=" + _account.Token + "&ads_manager_write_regions=true&method=post";
        HttpResponseMessage response = await client.PostAsync(url, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if (!(await response.Content.ReadAsStringAsync()).Contains("errors"))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> RemoveCreditCard(string adAccountId, string paymentMethodId)
    {
        try
        {
            // Fetch payment method when not provided
            if (string.IsNullOrEmpty(paymentMethodId))
            {
                HttpResponseMessage fetchPaymentResponse = await client.GetAsync("https://graph.facebook.com/graphql?method=post&locale=en_US&pretty=false&format=json&fb_api_req_friendly_name=BillingPayNowLandingScreenQuery&fb_api_caller_class=RelayModern&doc_id=6606214039405288&server_timestamps=true&variables={\"intent\":\"PAY_NOW\",\"paymentAccountID\":\"" + adAccountId + "\"}&access_token=" + _account.Token);
                MediaTypeHeaderValue fetchContentType = fetchPaymentResponse.Content.Headers.ContentType;
                string fetchCharset = ((fetchContentType != null) ? fetchContentType.CharSet : null);
                if (!string.IsNullOrEmpty(fetchCharset) && fetchCharset.Contains("\"utf-8\""))
                {
                    fetchPaymentResponse.Content.Headers.ContentType.CharSet = "utf-8";
                }
                JObject paymentInfo = JObject.Parse(await fetchPaymentResponse.Content.ReadAsStringAsync());
                paymentMethodId = paymentInfo?["data"]?["billable_account_by_payment_account"]?["billing_payment_account"]?["billing_payment_methods"]?[0]?["credential"]?["credential_id"]?.ToString();
                if (string.IsNullOrEmpty(paymentMethodId))
                {
                    return false;
                }
            }
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "__a", "1" },
                { "dpr", "1" },
                { "jazoest", "25313" },
                { "lsd", _account.LSDToken ?? string.Empty },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "useBillingRemovePMMutation" },
                { "doc_id", "6325673510865212" },
                {
                    "variables",
                    "{\"input\":{\"logging_data\":{\"logging_counter\":5,\"logging_id\":\"1947521442\"},\"payment_account_id\":\"" + adAccountId + "\",\"payment_method_id\":\"" + paymentMethodId + "\",\"upl_logging_data\":{\"context\":\"billingremovepm\",\"credential_id\":\"" + paymentMethodId + "\",\"entry_point\":\"BILLING_HUB\",\"external_flow_id\":\"627099224\",\"target_name\":\"useBillingRemovePMMutation\",\"wizard_config_name\":\"REMOVE_PM\",\"wizard_name\":\"REMOVE_PM\",\"wizard_screen_name\":\"remove_pm_state_display\",\"wizard_state_name\":\"remove_pm_state_display\"},\"actor_id\":\"" + _account.Uid + "\",\"client_mutation_id\":\"4\"}}"
                },
                { "fb_dtsg", _account.DTSGToken }
            };
            HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string charset = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(charset) && charset.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseStr) || responseStr.Contains("errors"))
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<string> GetTokenEAAB(string adAccountId)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("https://adsmanager.facebook.com/adsmanager/manage/ads?act=" + adAccountId);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string charset = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(charset) && charset.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            string token = Regex.Match(responseStr, "accessToken=\\\"(.*?)\\\"").Groups[1].Value;
            if (string.IsNullOrEmpty(token))
            {
                string redirectUrl = Regex.Match(responseStr, "location.replace\\(\\\"(.*?)\\\"").Groups[1].Value.Replace("\\", "");
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    response = await client.GetAsync(redirectUrl);
                    MediaTypeHeaderValue contentType2 = response.Content.Headers.ContentType;
                    charset = ((contentType2 != null) ? contentType2.CharSet : null);
                    if (!string.IsNullOrEmpty(charset) && charset.Contains("\"utf-8\""))
                    {
                        response.Content.Headers.ContentType.CharSet = "utf-8";
                    }
                    responseStr = await response.Content.ReadAsStringAsync();
                    token = Regex.Match(responseStr, "accessToken=\\\"(.*?)\\\"").Groups[1].Value;
                }
            }
            return token;
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<bool> ArchiveAllCampaigns(string adAccountId)
    {
        try
        {
            string tokenEAAB = await GetTokenEAAB(adAccountId);
            if (string.IsNullOrEmpty(tokenEAAB))
            {
                return false;
            }
            HttpResponseMessage draftResponse = await client.GetAsync("https://graph.facebook.com/v17.0/act_" + adAccountId + "/current_addrafts?fields=addraft_fragments{ad_object_type}&/current_addrafts?fields=addraft_fragments{ad_object_type}&access_token=" + tokenEAAB);
            if (!draftResponse.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue draftContentType = draftResponse.Content.Headers.ContentType;
            string draftCharset = ((draftContentType != null) ? draftContentType.CharSet : null);
            if (!string.IsNullOrEmpty(draftCharset) && draftCharset.Contains("\"utf-8\""))
            {
                draftResponse.Content.Headers.ContentType.CharSet = "utf-8";
            }
            JObject draftObject = JObject.Parse(await draftResponse.Content.ReadAsStringAsync());
            JArray draftData = draftObject?["data"] as JArray;
            if (draftData == null || draftData.Count == 0)
            {
                return false;
            }
            string draftId = draftData[0]?["id"]?.ToString();
            if (string.IsNullOrEmpty(draftId))
            {
                return false;
            }
            HttpResponseMessage adsResponse = await client.GetAsync("https://adsmanager-graph.facebook.com/v20.0/act_" + adAccountId + "/ads?fields=[\"id\",\"name\",\"campaign_id\",\"adset_id\", \"ad_id\"]&access_token=" + tokenEAAB);
            if (!adsResponse.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue adsContentType = adsResponse.Content.Headers.ContentType;
            string adsCharset = ((adsContentType != null) ? adsContentType.CharSet : null);
            if (!string.IsNullOrEmpty(adsCharset) && adsCharset.Contains("\"utf-8\""))
            {
                adsResponse.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string adsJson = await adsResponse.Content.ReadAsStringAsync();
            JArray ads = JObject.Parse(adsJson)["data"] as JArray;
            if (ads == null || ads.Count == 0)
            {
                return false;
            }
            foreach (JToken adItem in ads)
            {
                string campaignId = adItem["campaign_id"]?.ToString();
                string adSetId = adItem["adset_id"]?.ToString();
                string adId = adItem["id"]?.ToString();
                if (string.IsNullOrEmpty(campaignId) || string.IsNullOrEmpty(adSetId) || string.IsNullOrEmpty(adId))
                {
                    continue;
                }
                Dictionary<string, string> campaignPayload = new Dictionary<string, string>
                {
                    { "account_id", adAccountId },
                    { "action", "modify" },
                    { "ad_object_id", campaignId },
                    { "ad_object_type", "campaign" },
                    { "include_headers", "false" },
                    { "is_archive", "false" },
                    { "is_delete", "false" },
                    { "locale", "en_GB" },
                    { "method", "post" },
                    { "parent_ad_object_id", adAccountId },
                    { "pretty", "0" },
                    { "suppress_http_code", "1" },
                    { "validate", "false" },
                    { "values", "[{\"field\":\"status\",\"old_value\":\"ACTIVE\",\"new_value\":\"ARCHIVED\"}]" }
                };
                HttpResponseMessage campaignResponse = await client.PostAsync("https://adsmanager-graph.facebook.com/v20.0/" + draftId + "/addraft_fragments?_reqName=object%3Aaddraft%2Faddraft_fragments&access_token=" + tokenEAAB + "&method=post&__cppo=1&_callFlowletID=0&_triggerFlowletID=22293&qpl_active_flow_instance_ids=270206350_b3fe4dc7d5bbe08bc02,270214832_b3f72abb679173d9524", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)campaignPayload));
                if (!campaignResponse.IsSuccessStatusCode)
                {
                    return false;
                }
                string fragmentCampaignId = JObject.Parse(await campaignResponse.Content.ReadAsStringAsync())["id"].ToString();
                Dictionary<string, string> adsetPayload = new Dictionary<string, string>
                {
                    { "account_id", adAccountId },
                    { "action", "modify" },
                    { "ad_object_id", adSetId },
                    { "ad_object_type", "ad_set" },
                    { "include_headers", "false" },
                    { "is_archive", "false" },
                    { "is_delete", "false" },
                    { "locale", "en_GB" },
                    { "method", "post" },
                    { "parent_ad_object_id", campaignId },
                    { "pretty", "0" },
                    { "suppress_http_code", "1" },
                    { "validate", "false" },
                    { "values", "[{\"field\":\"status\",\"old_value\":\"ACTIVE\",\"new_value\":\"ARCHIVED\"}]" }
                };
                HttpResponseMessage adsetResponse = await client.PostAsync("https://adsmanager-graph.facebook.com/v20.0/" + draftId + "/addraft_fragments?_reqName=object%3Aaddraft%2Faddraft_fragments&access_token=" + tokenEAAB + "&method=post&__cppo=1&_callFlowletID=0&_triggerFlowletID=22293&qpl_active_flow_instance_ids=270206350_b3fe4dc7d5bbe08bc02,270214832_b3f72abb679173d9524", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)adsetPayload));
                if (!adsetResponse.IsSuccessStatusCode)
                {
                    return false;
                }
                string fragmentAdsetId = JObject.Parse(await adsetResponse.Content.ReadAsStringAsync())["id"].ToString();
                Dictionary<string, string> adPayload = new Dictionary<string, string>
                {
                    { "account_id", adAccountId },
                    { "action", "modify" },
                    { "ad_object_id", adId },
                    { "ad_object_type", "ad" },
                    { "include_headers", "false" },
                    { "is_archive", "false" },
                    { "is_delete", "false" },
                    { "locale", "en_GB" },
                    { "method", "post" },
                    { "parent_ad_object_id", adSetId },
                    { "pretty", "0" },
                    { "suppress_http_code", "1" },
                    { "validate", "false" },
                    { "values", "[{\"field\":\"status\",\"old_value\":\"ACTIVE\",\"new_value\":\"ARCHIVED\"}]" }
                };
                HttpResponseMessage adResponse = await client.PostAsync("https://adsmanager-graph.facebook.com/v20.0/" + draftId + "/addraft_fragments?_reqName=object%3Aaddraft%2Faddraft_fragments&access_token=" + tokenEAAB + "&method=post&__cppo=1&_callFlowletID=0&_triggerFlowletID=22293&qpl_active_flow_instance_ids=270206350_b3fe4dc7d5bbe08bc02,270214832_b3f72abb679173d9524", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)adPayload));
                if (!adResponse.IsSuccessStatusCode)
                {
                    return false;
                }
                string fragmentAdId = JObject.Parse(await adResponse.Content.ReadAsStringAsync())["id"].ToString();
                Dictionary<string, string> publishPayload = new Dictionary<string, string>
                {
                    { "fragments", "[\"" + fragmentAdId + "\",\"" + fragmentAdsetId + "\",\"" + fragmentCampaignId + "\"]" },
                    { "ignore_errors", "true" },
                    { "include_fragment_statuses", "true" },
                    { "include_headers", "false" },
                    { "locale", "en_US" },
                    { "method", "post" },
                    { "pretty", "0" },
                    { "qpl_active_flow_ids", "270208286,270208286,270216423" },
                    { "qpl_active_flow_instance_ids", "270208286_c8f24b7dd58d8b33e81,270216423_c8f875366c6ab07ceb3" },
                    { "suppress_http_code", "1" },
                    { "xref", "fcdfbf29bba1c384b" }
                };
                HttpResponseMessage publishResponse = await client.PostAsync("https://adsmanager-graph.facebook.com/v19.0/" + draftId + "/publish?_reqName=object%3Adraft_id%2Fpublish&access_token=" + tokenEAAB + "&method=post&qpl_active_flow_ids=270208286%2C270208286%2C270216423&qpl_active_flow_instance_ids=270208286_c8f24b7dd58d8b33e81%2C270216423_c8f875366c6ab07ceb3&__cppo=1&_callFlowletID=13632&_triggerFlowletID=13633", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)publishPayload));
                if (!publishResponse.IsSuccessStatusCode)
                {
                    return false;
                }
                string publishContent = await publishResponse.Content.ReadAsStringAsync();
                if (publishContent.Contains("error"))
                {
                    return false;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeletePartner(string adAccountId, string businessId, string partnerId)
    {
        string assetId = await GetAsset(adAccountId);
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "__a", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25421" },
            { "lsd", _account.LSDToken },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "BizKitSettingsRemovePartnerFromAssetMutation" },
            {
                "variables",
                "{\"businessID\":\"" + businessId + "\",\"assetID\":\"" + assetId + "\",\"partnerBusinessID\":\"" + partnerId + "\"}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "25037788389191427" }
        };
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://business.facebook.com/api/graphql/?_callFlowletID=10677&_triggerFlowletID=10672&qpl_active_e2e_trace_ids=");
        ((HttpHeaders)requestMessage.Headers).Add("x-fb-lsd", "6rcZCHasNekDQJU_hmWq6J");
        ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-site", "same-origin");
        ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-mode", "cors");
        requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload);
        HttpResponseMessage response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        if (!responseStr.Contains("error"))
        {
            return true;
        }
        if (responseStr.Contains("\"code\":2859009"))
        {
            if (string.IsNullOrEmpty(_account.Key2FA))
            {
                return false;
            }
            string code = Common.GetCode(_account.Key2FA);
            Dictionary<string, string> payloadVerifyCode = new Dictionary<string, string>
            {
                { "approvals_code", code },
                { "save_device", "false" },
                { "hash", "" },
                { "__aaid", "0" },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "__req", "6" },
                { "__hs", "20281.BP:DEFAULT.2.0...0" },
                { "dpr", "1" },
                { "__ccg", "EXCELLENT" },
                { "__rev", "1024691666" },
                { "__s", "v69om1:ii45zd:aazj17" },
                { "__hsi", "7526098785714860887" },
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25133" },
                { "lsd", "DPyYVDMXTBviNmF7ZRS8Cs" },
                { "__spin_r", "1024691666" },
                { "__spin_b", "trunk" },
                { "__spin_t", "1752306424" },
                { "__jssesw", "1" }
            };
            response = await client.PostAsync("https://business.facebook.com/security/twofactor/reauth/enter/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadVerifyCode));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
            contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
            response = await client.SendAsync(requestMessage);
            MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
            contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"success\":true"))
            {
                return true;
            }
        }
        return false;
    }

    private async Task<string> GetAsset(string adAccountId)
    {
        Dictionary<string, string> payloadGetAssetId = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "__a", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25421" },
            { "lsd", _account.LSDToken },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useBillingWizardQuery" },
            {
                "variables",
                "{\"paymentAccountID\":\"" + adAccountId + "\",\"universes\":[{\"params\":[\"xmds_enabled\"],\"type\":\"PAYMENT_ACCOUNT\",\"universe_name\":\"billing_react_xmds_migration_add_pm_nux\"},{\"params\":[\"xmds_enabled\"],\"type\":\"PAYMENT_ACCOUNT\",\"universe_name\":\"billing_react_xmds_migration_add_pm_pux\"},{\"params\":[\"xmds_enabled\"],\"type\":\"PAYMENT_ACCOUNT\",\"universe_name\":\"billing_react_xmds_migration_add_funds\"},{\"params\":[\"xmds_enabled\"],\"type\":\"PAYMENT_ACCOUNT\",\"universe_name\":\"billing_react_xmds_migration_pay_now\"},{\"params\":[\"xmds_enabled\"],\"type\":\"PAYMENT_ACCOUNT\",\"universe_name\":\"billing_react_xmds_migration_catch_all\"},{\"params\":[\"use_iframe\"],\"type\":\"PAYMENT_ACCOUNT\",\"universe_name\":\"billing_wizard_dialog_iframe\"}],\"gks\":[{\"name\":\"BILLING_REACT_XMDS_MIGRATION_EXPEDITED_ROLLOUT\",\"type\":\"PAYMENT_ACCOUNT_ID\"}]}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "24233492149685563" }
        };
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://business.facebook.com/api/graphql/?_callFlowletID=10677&_triggerFlowletID=10672&qpl_active_e2e_trace_ids=");
        ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-site", "same-origin");
        ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-mode", "cors");
        ((HttpHeaders)requestMessage.Headers).Add("x-fb-lsd", _account.LSDToken);
        requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadGetAssetId);
        HttpResponseMessage response = await client.SendAsync(requestMessage);
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject jobject = JObject.Parse(await response.Content.ReadAsStringAsync());
        return jobject["data"]["payment_account"]["billable_account"]["id"].ToString();
    }

    public async Task<(bool, string)> DeleteUserBefore7Days(string userId, string businessId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "business_id", businessId },
            { "admin_id", userId },
            { "session_id", "2e942068-0721-40b7-a912-4f89f3a72b0e" },
            { "event_source", "PMD" },
            { "__aaid", "0" },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "1a" },
            { "__hs", "20290.HYP:bizweb_comet_pkg.2.1...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1024951822" },
            { "__s", "xabwaw:mday38:kmokwb" },
            { "__hsi", "7529370840580633245" },
            { "__comet_req", "11" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25615" },
            { "lsd", "PHu87Tjx3aXTUSjgykqlQ9" },
            { "__spin_r", "1024951822" },
            { "__spin_b", "trunk" },
            { "__jssesw", "1" },
            {
                "__spin_t",
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/business/asset_onboarding/business_remove_admin/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return (false, "Error");
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        if (responseStr.Contains("true"))
        {
            return (true, "");
        }
        JObject responseObject = JObject.Parse(responseStr.Replace("for (;;);", ""));
        return (false, responseObject["errorSummary"]?.ToString());
    }

    public async Task<bool> DownTask(string userId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "1a" },
            { "__hs", "20290.HYP:bizweb_comet_pkg.2.1...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1024951822" },
            { "__s", "xabwaw:mday38:kmokwb" },
            { "__hsi", "7529370840580633245" },
            { "__comet_req", "11" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25615" },
            { "lsd", "PHu87Tjx3aXTUSjgykqlQ9" },
            { "__spin_r", "1024951822" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1753068259" },
            { "__jssesw", "1" },
            { "__crn", "comet.bizweb.BusinessCometBizSuiteSettingsBusinessUsersRoute" },
            { "qpl_active_flow_ids", "68104667" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "BusinessAccountPermissionTasksForUserModalMutation" },
            {
                "variables",
                "{\"businessUserID\":\"" + userId + "\",\"business_account_task_ids\":[\"926381894526285\"],\"isUnifiedSettings\":false}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "23927855636851737" },
            { "fb_api_analytics_tags", "[\"qpl_active_flow_ids=68104667\"]" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=8340&_triggerFlowletID=8335&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("\"sso_migration_status\":\"MIGRATION_NOT_NEEDED\""))
        {
            return true;
        }
        return true;
    }

    public async Task<bool> ChangeNameBM(string businessId, string businessName)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25475" },
            { "lsd", "YkAIr48YnE7bcWUGi-D1qL" },
            { "fb_api_caller_class", "RelayModern" },
            { "qpl_active_flow_ids", "558504925" },
            { "fb_api_req_friendly_name", "BizKitSettingsUpdateBusinessBasicInfoMutation" },
            {
                "variables",
                "{\"input\":{\"client_mutation_id\":\"3\",\"actor_id\":\"" + _account.Uid + "\",\"business_id\":\"" + businessId + "\",\"business_name\":\"" + businessName + "\",\"primary_page_id\":null,\"entry_point\":\"BUSINESS_MANAGER_BUSINESS_INFO\"}}"
            },
            { "doc_id", "7893672220672612" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("\"name\":\"" + businessName + "\""))
        {
            return true;
        }
        return false;
    }

    public async Task<(int, int)> KichAppVer2(string businessId, int nudCount = 1)
    {
        await client.GetAsync($"https://graph.facebook.com/graphql?access_token={_account.Token}&method=post&doc_id=9763356653753255&variables={{\"input\":{{\"client_mutation_id\":\"2\",\"actor_id\":\"0\",\"business_id\":\"{businessId}\",\"app_id\":\"225181538219344\",\"log_session_id\":\"WBxP-{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}-966419143\",\"acceptance_source\":\"DEVELOPERS_FACEBOOK_COM\"}}}}&_callFlowletID=0&_triggerFlowletID=7003&qpl_active_e2e_trace_ids=");
        string baseName = "POWER 2K";
        int countSuccess = 0;
        int countFailed = 0;
        HttpResponseMessage response;
        for (int i = 0; i < nudCount; i++)
        {
            string name = $"{baseName} {i + 1}";
            response = await client.GetAsync($"https://graph.facebook.com/graphql?method=post&doc_id=29701466519469036&variables={{\"input\":{{\"client_mutation_id\":\"3\",\"actor_id\":\"{_account.Uid}\",\"app_id\":\"1275424410903228\",\"log_session_id\":\"WBxP--{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}-{Common.CreateRandomNumber(9)}\",\"business_id\":\"{businessId}\",\"api_account_type\":\"SELF\",\"creation_source\":\"BUSINESS_MANAGER\",\"friendly_name\":\"{name}\",\"timezone_id\":132,\"partner_business_id\":\"{businessId}\",\"product\":\"SELF\"}}}}&access_token={_account.Token}&_callFlowletID=0&_triggerFlowletID=4113&qpl_active_e2e_trace_ids=");
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"billing_payment_account\":{\"__typename\":\"WhatsAppBusinessPaymentAccount\""))
            {
                countSuccess++;
            }
            else
            {
                countFailed++;
            }
        }
        response = await client.GetAsync("https://graph.facebook.com/v20.0/" + businessId + "?fields=owned_whatsapp_business_accounts.limit(200){id}&access_token=" + _account.Token + "&_callFlowletID=0&_triggerFlowletID=7052&qpl_active_e2e_trace_ids=");
        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
        string contentType4 = ((contentType3 != null) ? contentType3.CharSet : null);
        if (!string.IsNullOrEmpty(contentType4) && contentType4.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject ownerWaObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        JArray ownerWaBusinessAccountJarr = ownerWaObject["owned_whatsapp_business_accounts"]?["data"]?.ToObject<JArray>();
        foreach (JObject ownerWaBusinessAccountObjcet in ownerWaBusinessAccountJarr)
        {
            string id = ownerWaBusinessAccountObjcet["id"].ToString();
            await client.GetAsync("https://graph.facebook.com/graphql?access_token=" + _account.Token + "&method=post&doc_id=9902575596489138&variables={\"input\":{\"client_mutation_id\":\"8\",\"actor_id\":\"0\",\"waba_id\":\"" + id + "\"}}&_callFlowletID=0&_triggerFlowletID=7052&qpl_active_e2e_trace_ids=");
        }
        return (countSuccess, countFailed);
    }

    public async Task<(int, int)> CreateWA2(string businessId, int countWA = 1)
    {
        HttpResponseMessage response = await client.GetAsync("https://business.facebook.com/latest/settings/whatsapp_account?business_id=" + businessId);
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        _account.Uid = Regex.Match(responseStr, "USER_ID\":\"(.*?)\"").Groups[1].Value;
        _account.DTSGToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
        string baseName = "NB ADS";
        int countSuccess = 0;
        int countFailed = 0;
        for (int i = 0; i < countWA; i++)
        {
            string name = baseName + "_" + Common.CreateRandomNumber(3);
            JObject jObject = new JObject
            {
                ["input"] = new JObject
                {
                    ["client_mutation_id"] = "3",
                    ["actor_id"] = _account.Uid,
                    ["app_id"] = "225181538219344",
                    ["log_session_id"] = $"WBxP--{DateTimeOffset.Now.ToUnixTimeMilliseconds()}-1213723823",
                    ["business_id"] = businessId,
                    ["api_account_type"] = "SELF",
                    ["creation_source"] = "BUSINESS_MANAGER",
                    ["friendly_name"] = name,
                    ["timezone_id"] = 132,
                    ["primary_funding_source"] = null,
                    ["on_behalf_of_business_id"] = null,
                    ["partner_business_id"] = businessId,
                    ["page_id"] = null,
                    ["product"] = "SELF",
                    ["disable_automatic_sharing"] = null,
                    ["obo_onboarding_info_input"] = null
                }
            };
            string variable = Uri.EscapeDataString(jObject.ToString(Formatting.None));
            response = await client.GetAsync("https://graph.facebook.com/graphql?method=post&locale=en_US&pretty=false&format=json&fb_api_req_friendly_name=useCreateWhatsAppBusinessAPIAccountMutation_CreateWhatsAppBusinessAPIAccountMutation&doc_id=29701466519469036&fb_api_caller_class=RelayModern&server_timestamps=true&variables=" + variable + "&access_token=" + _account.Token);
            MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
            contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"billing_payment_account\":{\"__typename\":\"WhatsAppBusinessPaymentAccount\""))
            {
                countSuccess++;
            }
            else
            {
                countFailed++;
            }
        }
        return (countSuccess, countFailed);
    }

    public async Task DeleteAllWA(string businessId)
    {
        HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v20.0/" + businessId + "?fields=owned_whatsapp_business_accounts.limit(200){id}&access_token=" + _account.Token + "&_callFlowletID=0&_triggerFlowletID=7052&qpl_active_e2e_trace_ids=");
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject ownerWaObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        JArray ownerWaBusinessAccountJarr = ownerWaObject["owned_whatsapp_business_accounts"]?["data"]?.ToObject<JArray>();
        if (ownerWaBusinessAccountJarr == null)
        {
            return;
        }
        foreach (JObject ownerWaBusinessAccountObjcet in ownerWaBusinessAccountJarr)
        {
            string id = ownerWaBusinessAccountObjcet["id"].ToString();
            await client.GetAsync("https://graph.facebook.com/graphql?access_token=" + _account.Token + "&method=post&doc_id=9902575596489138&variables={\"input\":{\"client_mutation_id\":\"8\",\"actor_id\":\"0\",\"waba_id\":\"" + id + "\"}}&_callFlowletID=0&_triggerFlowletID=7052&qpl_active_e2e_trace_ids=");
        }
    }

    public async Task<JArray> CheckWA(string businessId)
    {
        HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v20.0/" + businessId + "?fields=owned_whatsapp_business_accounts.limit(200){id}&access_token=" + _account.Token + "&_callFlowletID=0&_triggerFlowletID=7052&qpl_active_e2e_trace_ids=");
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject ownerWaObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        return ownerWaObject["owned_whatsapp_business_accounts"]["data"].ToObject<JArray>();
    }

    public async Task<string> GetMyBusinessUserId(string businessId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__a", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25201" },
            { "lsd", _account.LSDToken }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/confirm_business/assets/?business_id=" + businessId + "&session_id=2e588b28-ebe5-43ce-b6fb-444a47b24bfc&see_all_assets=false", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = (await response.Content.ReadAsStringAsync()).Replace("for (;;);", "");
        JToken payloadObject = JObject.Parse(responseStr)["payload"];
        JToken businessAdmins = payloadObject["businessAdmins"]["__imm"]["value"].ToObject<JArray>().FirstOrDefault((JToken x) => x["id"].ToString() == _account.Uid);
        if (businessAdmins == null)
        {
            return null;
        }
        string name = businessAdmins["name"].ToString();
        return payloadObject["businessAssets"]["__imm"]["value"].ToObject<JArray>().FirstOrDefault((JToken x) => x["name"].ToString() == name)?["id"].ToString();
    }

    public async Task<bool> AssignFinanceRole(string businessUserId)
    {
        if (!(await client.GetAsync("https://graph.facebook.com/graphql?access_token=" + _account.Token + "&method=post&doc_id=23927855636851737&variables={\"businessUserID\":\"" + businessUserId + "\",\"business_account_task_ids\":[\"926381894526285\",\"388517145453246\",\"245181923290198\",\"768085000593466\",\"416103972652535\",\"603931664885191\",\"1327662214465567\",\"862159105082613\",\"6161001899617846786\",\"1633404653754086\",\"967306614466178\",\"2848818871965443\"],\"isUnifiedSettings\":false}&_callFlowletID=0&_triggerFlowletID=7003&qpl_active_e2e_trace_ids=")).IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> AddAdAccountIntoBM(string businessId, string adAccountId)
    {
        try
        {
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "__user", _account.Uid },
                { "__a", "1" },
                { "dpr", "1" },
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25620" },
                { "lsd", _account.LSDToken }
            };
            string url = "https://business.facebook.com/business/objects/add/connections/?business_id=" + businessId + "&from_id=" + businessId + "&from_asset_type=brand&to_id=" + adAccountId + "&to_asset_type=ad-account";
            HttpResponseMessage response = await client.PostAsync(url, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"success\":true"))
            {
                return true;
            }
        }
        catch
        {
        }
        return false;
    }

    public async Task KichApp3(string businessId)
    {
        string url = "https://www.facebook.com/v14.0/dialog/oauth?app_id=799369954601524&cbt=1754199337031&channel_url=https%3A%2F%2Fstaticxx.facebook.com%2Fx%2Fconnect%2Fxd_arbiter%2F%3Fversion%3D46%23cb%3Df62db6424afc99dfb%26domain%3Dwww.app.aisensy.com%26is_canvas%3Dfalse%26origin%3Dhttps%253A%252F%252Fwww.app.aisensy.com%252Ff5348b90ca7de50fe%26relation%3Dopener&client_id=799369954601524&config_id=1269952934120839&display=popup&domain=www.app.aisensy.com&e2e=%7B%7D&extras=%7B%22sessionInfoVersion%22%3A%223%22%2C%22feature%22%3A%22whatsapp_embedded_signup%22%2C%22features%22%3A[%7B%22name%22%3A%22marketing_messages_lite%22%7D%2C%7B%22name%22%3A%22will_be_partner_certified%22%7D]%2C%22setup%22%3A%7B%22business%22%3A%7B%22isWebsiteRequired%22%3Afalse%2C%22name%22%3A%22WeMake%22%2C%22email%22%3A%22phanvinhhai8888%40gmail.com%22%2C%22phone%22%3A%7B%22code%22%3A91%2C%22number%22%3A%2212679868001%22%7D%2C%22address%22%3A%7B%22streetAddress1%22%3A%22%22%2C%22city%22%3A%22%22%2C%22state%22%3A%22%22%2C%22zipPostal%22%3A%22%22%2C%22country%22%3A%22%22%7D%2C%22timezone%22%3A%22UTC%2B05%3A30%22%7D%2C%22phone%22%3A%7B%22displayName%22%3A%22WeMake%22%2C%22category%22%3A%22%22%2C%22description%22%3A%22%22%7D%7D%7D&fallback_redirect_uri=https%3A%2F%2Fwww.app.aisensy.com%2Fprojects%2F688ef549f8ca5d0c13344d24%2Fdashboard&locale=en_US&logger_id=fafe7e6a18565de7c&origin=1&override_default_response_type=true&redirect_uri=https%3A%2F%2Fstaticxx.facebook.com%2Fx%2Fconnect%2Fxd_arbiter%2F%3Fversion%3D46%23cb%3Df0f2a5935150297a6%26domain%3Dwww.app.aisensy.com%26is_canvas%3Dfalse%26origin%3Dhttps%253A%252F%252Fwww.app.aisensy.com%252Ff5348b90ca7de50fe%26relation%3Dopener%26frame%3Df9a499975fc30cc4a&response_type=code&sdk=joey&version=v14.0";
        HttpResponseMessage response = await client.GetAsync(url);
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> lg1 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "2" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026555741" },
            { "__s", "3umye6:uo6bsm:l7ic9y" },
            { "__hsi", "7544944078702525255" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25533" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026555741" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756694186" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useGetWhatsAppBusinessPartnerTypeQuery" },
            { "variables", "{\"input\":{\"app_id\":\"799369954601524\",\"log_session_id\":\"fafe7e6a18565de7c\"}}" },
            { "server_timestamps", "true" },
            { "doc_id", "9113402172095278" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)lg1));
        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
        contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> lg2 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "4" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", "ew526w:hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBizxPlatformMultiPartnerQuery_WhatsAppBizxPlatformMultiPartnerQuery" },
            { "variables", "{\"input\":{\"app_id\":\"799369954601524\",\"log_session_id\":\"fafe7e6a18565de7c\"}}" },
            { "server_timestamps", "true" },
            { "doc_id", "9951076094957014" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)lg2));
        MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
        contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> tieptuc1 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "6" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBusinessAccountContainerQuery_WhatsAppBusinessAccountContainerQuery" },
            { "variables", "{}" },
            { "server_timestamps", "true" },
            { "doc_id", "9998854920237838" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)tieptuc1));
        MediaTypeHeaderValue contentType5 = response.Content.Headers.ContentType;
        contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> tieptuc2 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "7" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useCheckMetaBusinessRestrictionsLazyLoadQuery" },
            {
                "variables",
                "{\"input\":{\"business_id\":\"" + businessId + "\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "23903269282630861" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)tieptuc2));
        MediaTypeHeaderValue contentType6 = response.Content.Headers.ContentType;
        contentType2 = ((contentType6 != null) ? contentType6.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> tieptuc3 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "8" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useCheckWhatsAppPartnerWabaOnboardingEligibilityQuery" },
            { "variables", "{\"input\":{\"app_id\":\"799369954601524\",\"log_session_id\":\"fafe7e6a18565de7c\",\"partner_business_id\":1215672488927258}}" },
            { "server_timestamps", "true" },
            { "doc_id", "24330727156530677" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)tieptuc3));
        MediaTypeHeaderValue contentType7 = response.Content.Headers.ContentType;
        contentType2 = ((contentType7 != null) ? contentType7.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> tieptuc4 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "9" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBizxPlatformGetBusinessInformationQuery" },
            {
                "variables",
                "{\"businessID\":\"" + businessId + "\"}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "24118172981100910" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)tieptuc4));
        MediaTypeHeaderValue contentType8 = response.Content.Headers.ContentType;
        contentType2 = ((contentType8 != null) ? contentType8.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> tieptuc5 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "a" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBizxPlatformGetBusinessTOSEligibilityQuery" },
            {
                "variables",
                "{\"input\":{\"business_id\":\"" + businessId + "\",\"log_session_id\":\"fafe7e6a18565de7c\",\"surface\":\"EMBEDDED_SIGNUP\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "29668132816164201" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)tieptuc5));
        MediaTypeHeaderValue contentType9 = response.Content.Headers.ContentType;
        contentType2 = ((contentType9 != null) ? contentType9.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "e" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useUpdateMetaBusinessAccountMutation_MetaBusinessAccountMutation" },
            {
                "variables",
                "{\"input\":{\"client_mutation_id\":\"1\",\"actor_id\":\"" + _account.Uid + "\",\"app_id\":\"799369954601524\",\"log_session_id\":\"fafe7e6a18565de7c\",\"business_id\":\"" + businessId + "\",\"entry_point\":\"WHATSAPP_BUSINESS_ONBOARDING_EMBEDDED_SIGNUP_BUSINESS_ACCOUNT\",\"email_address\":\"\",\"business_profile\":{\"legal_name\":\"WA\",\"website\":\"\",\"address\":{\"country\":\"US\"}}}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "24129681633295981" },
            { "fb_api_analytics_tags", "[\"qpl_active_flow_ids=606156028\"]" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp));
        MediaTypeHeaderValue contentType10 = response.Content.Headers.ContentType;
        contentType2 = ((contentType10 != null) ? contentType10.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp5 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "g" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useGetPartnerOnboardingRestrictionQuery" },
            {
                "variables",
                "{\"input\":{\"end_business_id\":\"" + businessId + "\",\"app_id\":\"799369954601524\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9909233332492327" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp5));
        MediaTypeHeaderValue contentType11 = response.Content.Headers.ContentType;
        contentType2 = ((contentType11 != null) ? contentType11.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp6 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "i" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useAcceptOptimizedDeliveryApiTosMutationMutation" },
            {
                "variables",
                "{\"input\":{\"client_mutation_id\":\"2\",\"actor_id\":\"" + _account.Uid + "\",\"business_id\":\"" + businessId + "\",\"app_id\":\"799369954601524\",\"log_session_id\":\"fafe7e6a18565de7c\",\"acceptance_source\":\"CLOUD_API_AND_MM_LITE_ES\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9763356653753255" },
            { "fb_api_analytics_tags", "[\"qpl_active_flow_ids=606156028\"]" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp6));
        MediaTypeHeaderValue contentType12 = response.Content.Headers.ContentType;
        contentType2 = ((contentType12 != null) ? contentType12.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp7 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "j" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useAcceptWhatsAppBusinessCloudApiTermsOfServiceMutation" },
            {
                "variables",
                "{\"input\":{\"client_mutation_id\":\"3\",\"actor_id\":\"" + _account.Uid + "\",\"log_session_id\":\"fafe7e6a18565de7c\",\"business_id\":\"" + businessId + "\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9071212459646264" },
            { "fb_api_analytics_tags", "[\"qpl_active_flow_ids=606156028\"]" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp7));
        MediaTypeHeaderValue contentType13 = response.Content.Headers.ContentType;
        contentType2 = ((contentType13 != null) ? contentType13.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp8 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "k" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBusinessBspInitiatedActionsQuery" },
            {
                "variables",
                "{\"input\":{\"app_id\":\"799369954601524\",\"business_id\":\"" + businessId + "\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9186433524789867" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp8));
        MediaTypeHeaderValue contentType14 = response.Content.Headers.ContentType;
        contentType2 = ((contentType14 != null) ? contentType14.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp9 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "l" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:tws6m8" },
            { "__hsi", "7544972786726184157" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25359" },
            { "lsd", "zcRkGy1Fr-pwm2BMgLpGO3" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756700870" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBusinessCheckVirtualPhoneNumberEligibilityQuery" },
            {
                "variables",
                "{\"input\":{\"log_session_id\":\"fafe7e6a18565de7c\",\"partner_business_id\":1215672488927258,\"business_id\":\"" + businessId + "\",\"platform_type\":\"ES\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "23977002918559493" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp9));
        MediaTypeHeaderValue contentType15 = response.Content.Headers.ContentType;
        contentType2 = ((contentType15 != null) ? contentType15.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp10 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "m" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:2wwrok" },
            { "__hsi", "7544991161075030207" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25515" },
            { "lsd", "WQnQqh3KJif6vEha4hADZo" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756705147" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBusinessGKCheckQuery" },
            {
                "variables",
                "{\"input\":{\"gk\":\"WA_BIZX_LIMITED_FIELDS_FOR_WABA_LIST\",\"business_id\":\"" + businessId + "\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9667988736602876" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp10));
        MediaTypeHeaderValue contentType16 = response.Content.Headers.ContentType;
        contentType2 = ((contentType16 != null) ? contentType16.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp11 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "n" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:2wwrok" },
            { "__hsi", "7544991161075030207" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25515" },
            { "lsd", "WQnQqh3KJif6vEha4hADZo" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            {
                "__spin_t",
                DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()
            },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBusinessAccountListLazyQuery" },
            {
                "variables",
                "{\"find_by\":{\"app_id\":\"799369954601524\",\"log_session_id\":\"fafe7e6a18565de7c\",\"business_id\":\"" + businessId + "\"},\"page_id\":null,\"on_behalf_of_business_id\":1215672488927258,\"partner_business_id\":1215672488927258,\"product_type\":\"EMBEDDED_SIGNUP\",\"ad_account_id\":\"\",\"skip_messaging_limit\":true,\"load_limited_fields\":false}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9413358905434267" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp11));
        MediaTypeHeaderValue contentType17 = response.Content.Headers.ContentType;
        contentType2 = ((contentType17 != null) ? contentType17.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp12 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "p" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:2wwrok" },
            { "__hsi", "7544991161075030207" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25515" },
            { "lsd", "WQnQqh3KJif6vEha4hADZo" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756705147" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBusinessLazyLoadAccountTier_WhatsAppBusinessAccountTierQuery" },
            { "variables", "{\"business_id\":\"1088081283512347\"}" },
            { "server_timestamps", "true" },
            { "doc_id", "9729460900470094" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp12));
        MediaTypeHeaderValue contentType18 = response.Content.Headers.ContentType;
        contentType2 = ((contentType18 != null) ? contentType18.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp13 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "q" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:2wwrok" },
            { "__hsi", "7544991161075030207" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25515" },
            { "lsd", "WQnQqh3KJif6vEha4hADZo" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756705147" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useCheckMetaBusinessRestrictionsLazyLoadQuery" },
            { "variables", "{\"input\":{\"business_id\":\"1088081283512347\"}}" },
            { "server_timestamps", "true" },
            { "doc_id", "23903269282630861" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp13));
        MediaTypeHeaderValue contentType19 = response.Content.Headers.ContentType;
        contentType2 = ((contentType19 != null) ? contentType19.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoapp14 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "s" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:2wwrok" },
            { "__hsi", "7544991161075030207" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25515" },
            { "lsd", "WQnQqh3KJif6vEha4hADZo" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756705147" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useWhatsAppBizxPlatformGetBusinessInformationQuery" },
            {
                "variables",
                "{\"businessID\":\"" + businessId + "\"}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "24118172981100910" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoapp14));
        MediaTypeHeaderValue contentType20 = response.Content.Headers.ContentType;
        contentType2 = ((contentType20 != null) ? contentType20.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        Dictionary<string, string> taoWA1 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "t" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:2wwrok" },
            { "__hsi", "7544991161075030207" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25515" },
            { "lsd", "WQnQqh3KJif6vEha4hADZo" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756705147" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useVerifyWhatsAppBusinessInfoQuery" },
            {
                "variables",
                "{\"input\":{\"actor_id\":\"" + _account.Uid + "\",\"app_id\":\"799369954601524\",\"log_session_id\":\"fafe7e6a18565de7c\",\"whatsapp_business_name\":\"WeMake\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9291705997600742" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoWA1));
        MediaTypeHeaderValue contentType21 = response.Content.Headers.ContentType;
        contentType2 = ((contentType21 != null) ? contentType21.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        JObject jObject = new JObject
        {
            ["input"] = new JObject
            {
                ["client_mutation_id"] = "3",
                ["actor_id"] = _account.Uid,
                ["app_id"] = "225181538219344",
                ["log_session_id"] = $"WBxP--{DateTimeOffset.Now.ToUnixTimeSeconds()}-1213723823",
                ["business_id"] = businessId,
                ["api_account_type"] = "SELF",
                ["creation_source"] = "BUSINESS_MANAGER",
                ["friendly_name"] = "POWER RANGER",
                ["timezone_id"] = 132,
                ["primary_funding_source"] = null,
                ["on_behalf_of_business_id"] = null,
                ["partner_business_id"] = businessId,
                ["page_id"] = null,
                ["product"] = "SELF",
                ["disable_automatic_sharing"] = null,
                ["obo_onboarding_info_input"] = null
            }
        };
        string varialables = Uri.EscapeDataString(jObject.ToString(Formatting.None));
        string address = "https://graph.facebook.com/graphql?method=post&locale=en_US&pretty=false&format=json&fb_api_req_friendly_name=useCreateWhatsAppBusinessAPIAccountMutation_CreateWhatsAppBusinessAPIAccountMutation&doc_id=29701466519469036&fb_api_caller_class=RelayModern&server_timestamps=true&variables=" + varialables + "&access_token=" + _account.Token;
        response = await client.GetAsync(address);
        MediaTypeHeaderValue contentType22 = response.Content.Headers.ContentType;
        contentType2 = ((contentType22 != null) ? contentType22.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject obj = JObject.Parse(await response.Content.ReadAsStringAsync());
        string id = (string?)obj["data"]?["xfb_create_whatsapp_business_api_account"]?["whatsapp_business_account"]?["id"];
        Dictionary<string, string> taoWA3 = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "w" },
            { "__hs", "20332.BP:DEFAULT.2.0...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1026556556" },
            { "__s", ":hs37ay:2wwrok" },
            { "__hsi", "7544991161075030207" },
            { "locale", "en_US" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25515" },
            { "lsd", "WQnQqh3KJif6vEha4hADZo" },
            { "__spin_r", "1026556556" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1756705147" },
            { "qpl_active_flow_ids", "606156028" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useGetPhoneNumbersUnderWabaLazyLoadQuery" },
            {
                "variables",
                "{\"wabaID\":\"" + id + "\"}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9383859391716235" }
        };
        response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)taoWA3));
        MediaTypeHeaderValue contentType23 = response.Content.Headers.ContentType;
        contentType2 = ((contentType23 != null) ? contentType23.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
    }

    public async Task CreateWA(string businessId, int nudWa = 4)
    {
        string baseName = "NB ADS";
        int successCount = 0;
        int failedCount = 0;
        for (int i = 0; i < nudWa; i++)
        {
            string name = $"{baseName}_{i}";
            JObject jObject = new JObject
            {
                ["input"] = new JObject
                {
                    ["client_mutation_id"] = "3",
                    ["actor_id"] = _account.Uid,
                    ["app_id"] = "225181538219344",
                    ["log_session_id"] = $"WBxP--{DateTimeOffset.Now.ToUnixTimeSeconds()}-1213723823",
                    ["business_id"] = businessId,
                    ["api_account_type"] = "SELF",
                    ["creation_source"] = "BUSINESS_MANAGER",
                    ["friendly_name"] = name,
                    ["timezone_id"] = 132,
                    ["primary_funding_source"] = null,
                    ["on_behalf_of_business_id"] = null,
                    ["partner_business_id"] = businessId,
                    ["page_id"] = null,
                    ["product"] = "SELF",
                    ["disable_automatic_sharing"] = null,
                    ["obo_onboarding_info_input"] = null
                }
            };
            string varialables = Uri.EscapeDataString(jObject.ToString(Formatting.None));
            string address = "https://graph.facebook.com/graphql?method=post&locale=en_US&pretty=false&format=json&fb_api_req_friendly_name=useCreateWhatsAppBusinessAPIAccountMutation_CreateWhatsAppBusinessAPIAccountMutation&doc_id=29701466519469036&fb_api_caller_class=RelayModern&server_timestamps=true&variables=" + varialables + "&access_token=" + _account.Token;
            HttpResponseMessage response = await client.GetAsync(address);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            if ((await response.Content.ReadAsStringAsync()).Contains("\"billing_payment_account\":{\"__typename\":\"WhatsAppBusinessPaymentAccount\""))
            {
                successCount++;
            }
            else
            {
                failedCount++;
            }
        }
    }

    public async Task<(string, int)> CheckLimitBM(string businessId)
    {
        HttpResponseMessage response = await HttpClientCommon.GetWithRetry(url: "https://graph.facebook.com/v7.0/" + businessId + "/owned_ad_accounts?fields=adtrust_dsl&limit=50&summary=total_count&access_token=" + _account.Token, client: client, account: _account);
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject adAccountObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        JArray adAccountIds = adAccountObject["data"]?.ToObject<JArray>();
        if (adAccountIds != null && adAccountIds.Count() > 0)
        {
            JToken adAccountFirstObject = adAccountIds.FirstOrDefault();
            string adtrust = adAccountFirstObject["adtrust_dsl"].ToString();
            int ownedAccountCount = adAccountIds.Count;
            return adtrust switch
            {
                "-1" => ("NoLimit", ownedAccountCount),
                "250" => ("250", ownedAccountCount),
                "50" => ("50", ownedAccountCount),
                _ => (adtrust, ownedAccountCount),
            };
        }
        return ("Không xác định", 0);
    }

    public async Task<bool> AddNewMail(string businessUserId, string businessId, string mail)
    {
        Dictionary<string, string> content = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "__req", "19" },
            { "__hs", "20315.HYP:bizweb_comet_pkg.2.1...0" },
            { "dpr", "1" },
            { "__ccg", "EXCELLENT" },
            { "__rev", "1025915895" },
            { "__s", "1odncs:mk00kb:hgcivi" },
            { "__hsi", "7538631945757327476" },
            { "__comet_req", "11" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25464" },
            { "lsd", "VX9kf7QVBMjDwEUapCp5ul" },
            { "__spin_r", "1025915895" },
            { "__spin_b", "trunk" },
            { "__spin_t", "1755224528" },
            { "__jssesw", "1" },
            { "__crn", "comet.bizweb.BusinessCometBizSuiteSettingsBusinessUsersRoute" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "GetBusinessSensitiveActionEnumQuery" },
            {
                "variables",
                "{\"businessUserID\":\"" + businessUserId + "\",\"firstName\":\"" + _account.Uid + "\",\"lastName\":\"1\",\"email\":\"" + mail + "\",\"clearPendingEmail\":null,\"surface_params\":{\"entry_point\":\"BIZWEB_SETTINGS_BUSINESS_INFO_TAB\",\"flow_source\":\"BIZ_WEB\",\"tab\":\"business_info\"},\"isFromEmailMismatchFlow\":null,\"isFromClaimRequestAccept\":null}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "10075214885931406" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=6794&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)content));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        return true;
    }

    public async Task<bool> DiscardInvite(string requestId)
    {
        if (!(await client.GetAsync("https://graph.facebook.com/graphql?access_token=" + _account.Token + "&method=post&doc_id=9598002093655175&variables={\"businessRoleRequestID\":\"" + requestId + "\"}&_callFlowletID=0&_triggerFlowletID=7003&qpl_active_e2e_trace_ids=")).IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteUser(string businessUserId, string businessId)
    {
        Dictionary<string, string> content = new Dictionary<string, string>
        {
            { "__activeScenarioIDs", "[]" },
            { "__activeScenarios", "[]" },
            { "__interactionsMetadata", "[]" },
            { "_reqName", "object/Abusiness_user" },
            { "__a", "1" },
            { "_reqSrc", "UserServerActions.brands" },
            { "locale", "en_US" },
            { "dpr", "1" },
            { "method", "delete" }
        };
        HttpResponseMessage response = await client.PostAsync("https://graph.facebook.com/v17.0/" + businessUserId + "?access_token=" + _account.Token, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)content));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        return true;
    }

    public async Task<bool> DeleteUserByIG(string businessUserId, string businessId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__aaid", "0" },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25326" },
            { "lsd", "vpdYdGKieX1e0jlSljR1l6" },
            { "doc_id", "9664231750291365" },
            {
                "variables",
                "{\"reviewParams\":{\"action_type\":\"BUSINESS_REMOVE_USER\",\"business_id\":\"" + businessId + "\",\"remove_user_params\":{\"target_user_id\":\"" + businessUserId + "\"}},\"roleRequestId\":\"\",\"isNotAddAdmin\":true}"
            }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("\"review_process\":\"NONE\""))
        {
            return true;
        }
        return false;
    }

    public async Task<string> CheckRetrictBM(string businessId)
    {
        HttpResponseMessage response = await client.GetAsync("https://business.facebook.com/business-support-home/" + businessId + "/?source=mbs_more_tools_flyout");
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string idesEnforcenmentInstanceId = Regex.Match(await response.Content.ReadAsStringAsync(), "idesEnforcementInstanceID\":\"(.*?)\"").Groups[1].Value;
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "__a", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "lsd", _account.LSDToken },
            {
                "variables",
                "{\"ides_enforcement_instance_id\":\"" + idesEnforcenmentInstanceId + "\",\"screen\":\"DETAIL\",\"entityID\":\"" + businessId + "\",\"entrypoint\":\"BSH_ENFORCED_ENTITY_PAGE\",\"scale\":1}"
            },
            { "doc_id", "24749019144706659" }
        };
        response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=1&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
        contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        if (responseStr.Contains("{\"errors\":[{\"message\""))
        {
            return "Die Cookie";
        }
        string appealStatus = Regex.Match(responseStr, "appeal_status\":\"(.*?)\"").Groups[1].Value;
        if (!string.IsNullOrEmpty(appealStatus))
        {
            if (appealStatus == "APPEAL_ENABLED_NOT_STARTED_YET")
            {
                return "CÓ NÚT";
            }
            if (appealStatus == "USER_STARTED_APPEALING")
            {
                return "ĐANG KHÁNG";
            }
            return "Vhh";
        }
        string appealStates = Regex.Match(responseStr, "appeal_state_value\":\"(.*?)\"").Groups[1].Value;
        string appealStateText = Regex.Match(responseStr, "appeal_state\":{\"text\":\"(.*?)\"").Groups[1].Value;
        string resultMessage = string.Empty;
        if (appealStateText == "Review complete")
        {
            resultMessage += "KHÁNG XONG-";
            resultMessage = ((appealStates == "APPEAL_REJECTED") ? (resultMessage + "Vhh") : ((!(appealStates == "APPEAL_APPROVED")) ? (resultMessage + appealStates) : (resultMessage + "XANH VỎ")));
        }
        else
        {
            resultMessage += "KHÔNG NÚT";
        }
        return resultMessage;
    }

    public async Task<bool> SetSpendCap(string adAccountId, string limit, string currency)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25583" },
            { "lsd", "bZSCQTo_tK4d6oBV7NE3D3" },
            { "qpl_active_flow_ids", "30619976" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useBillingUpdateAccountSpendLimitScreenMutation" },
            {
                "variables",
                "{\"input\":{\"billable_account_payment_legacy_account_id\":\"" + adAccountId + "\",\"logging_data\":{\"logging_counter\":6,\"logging_id\":\"333917393\"},\"new_spend_limit\":{\"amount\":\"" + limit + "\",\"currency\":\"" + currency + "\"},\"reset_period\":\"MONTHLY\",\"upl_logging_data\":{\"context\":\"billingspendlimits\",\"entry_point\":\"BILLING_HUB\",\"wizard_config_name\":\"UPDATE_ACCOUNT_SPEND_LIMIT\",\"wizard_name\":\"UPDATE_ACCOUNT_SPEND_LIMIT\",\"wizard_screen_name\":\"update_account_spend_limit_state_display\",\"wizard_state_name\":\"update_account_spend_limit_state_display\"},\"actor_id\":\"" + _account.Uid + "\",\"client_mutation_id\":\"14\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "7527437027276530" },
            { "fb_api_analytics_tags", "[\"qpl_active_flow_ids=30619976\"]" }
        };
        if (!(await client.PostAsync("https://business.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload))).IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> RemoveSpendCap(string adAccountId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25583" },
            { "lsd", "bZSCQTo_tK4d6oBV7NE3D3" },
            { "qpl_active_flow_ids", "30619976" },
            { "fb_api_caller_class", "RelayModern" },
            { "fb_api_req_friendly_name", "useBillingRemoveASLMutation" },
            {
                "variables",
                "{\"input\":{\"billable_account_payment_legacy_account_id\":\"" + adAccountId + "\",\"upl_logging_data\":{\"context\":\"billingspendlimits\",\"entry_point\":\"ads_manager\",\"external_flow_id\":\"upl_1761153166293_e573fede-29e6-40d2-ad0e-21fe57304405\",\"target_name\":\"useBillingRemoveASLMutation\",\"user_session_id\":\"upl_1761153166293_e573fede-29e6-40d2-ad0e-21fe57304405\",\"wizard_config_name\":\"REMOVE_ASL\",\"wizard_name\":\"REMOVE_ASL\",\"wizard_screen_name\":\"remove_asl_state_display\",\"wizard_session_id\":\"upl_wizard_1761153166293_f6562546-1a27-4b25-a1d9-221a6acc2905\"},\"actor_id\":\"100027025626973\",\"client_mutation_id\":\"7\"}}"
            },
            { "server_timestamps", "true" },
            { "doc_id", "9689753567808836" },
            { "fb_api_analytics_tags", "[\"qpl_active_flow_ids=30619976\"]" }
        };
        if (!(await client.PostAsync("https://business.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload))).IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task<string> CheckCampaign(string adAccountId)
    {
        HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v2.11/act_" + adAccountId + "/ads?fields=effective_status&limit=1000&access_token=" + _account.Token);
        if (response.IsSuccessStatusCode)
        {
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            JObject jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            JArray jArray = (JArray)jObject["data"];
            if (jArray.Count() == 0)
            {
                return "Không có camp";
            }
            string text = string.Empty;
            foreach (JToken item in jArray)
            {
                text = text + "-" + item["effective_status"].ToString().Replace("CAMPAIGN_", "");
            }
            return text.TrimStart('-');
        }
        return "Check Camp Lỗi!";
    }

    public async Task<bool> CheckIsPay(string adAccountId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "__a", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "lsd", _account.LSDToken },
            {
                "variables",
                "{\"paymentAccountID\":\"" + adAccountId + "\",\"count\":10,\"cursor\":null,\"skip_billing_reason\":false}"
            },
            { "doc_id", "24250565057963092" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=7500&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("status\":\"COMPLETED\""))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> Pay(string adAccountId)
    {
        HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/graphql?method=post&locale=en_US&pretty=false&format=json&fb_api_req_friendly_name=BillingPayNowLandingScreenQuery&fb_api_caller_class=RelayModern&doc_id=6606214039405288&server_timestamps=true&variables={\"intent\":\"PAY_NOW\",\"paymentAccountID\":\"" + adAccountId + "\"}&access_token=" + _account.Token);
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject val = JObject.Parse(await response.Content.ReadAsStringAsync());
        string text2 = val["data"]["billable_account_by_payment_account"]["account_balance"]["localAmountNoSymbol"].ToString();
        string text3 = val["data"]["billable_account_by_payment_account"]["account_balance"]["currency"].ToString();
        string credentialId = val["data"]?["billable_account_by_payment_account"]?["billing_payment_account"]?["billing_payment_methods"]?[0]["credential"]?["credential_id"]?.ToString();
        response = await client.GetAsync("https://graph.secure.facebook.com/graphql?method=post&locale=en_US&pretty=false&format=json&fb_api_req_friendly_name=BillingPayNowOrSettleStatePayNowMutation&fb_api_caller_class=RelayModern&doc_id=5553047091425712&server_timestamps=true&variables={\"input\":{\"billable_account_payment_legacy_account_id\":\"" + adAccountId + "\",\"credential_id\":\"" + credentialId + "\",\"payment_amount\":{\"amount\":\"" + text2 + "\",\"currency\":\"" + text3 + "\"},\"transaction_initiation_source\":\"CUSTOMER\",\"actor_id\":\"" + _account.Uid + "\",\"client_mutation_id\":\"2\"}}&access_token=" + _account.Token);
        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
        contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        if ((await response.Content.ReadAsStringAsync()).Contains("error"))
        {
            return false;
        }
        return true;
    }

    public async Task<(StatusJoinBM, string)> JoinByLinkInvite(string link, string fName, string lName, bool isCheck = false)
    {
        Exception exception = new Exception();
        try
        {
            string reauthLink = "https://m.facebook.com/password/reauth/?next=https%3A%2F%2Fmbasic.facebook.com%2Fsecurity%2F2fac%2Fsettings%2F%3Fpaipv%3D0%26eav%3DAfZfmwJnXhbeLP6m-giW1oCoZD0faAw6x_1LxHqf1nvS-tew9Vl6iEkBMuwwPNYH7Zw&paipv=0&eav=AfbC-ToI9zgklrUncTH4S-pXjfy5d5SPf9ZLf_iWIHepbPFg8mMnmmsnW0Or3AkCflI";
            Dictionary<string, string> payloadReauth = new Dictionary<string, string>
            {
                { "fb_dtsg", _account.DTSGToken },
                { "jazoest", "25494" },
                {
                    "encpass",
                    "#PWD_BROWSER:0:1111:" + _account.Password
                }
            };
            client.PostAsync(reauthLink, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadReauth));
            HttpResponseMessage response = await client.GetWithRetry(link, _account);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string responseStr = await response.Content.ReadAsStringAsync();
            if (response.RequestMessage.RequestUri.AbsolutePath == "/checkpoint/601051028565049/")
            {
                if (string.IsNullOrEmpty(_account.DTSGToken))
                {
                    _account.DTSGToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                }
                Dictionary<string, string> payload = new Dictionary<string, string>
                {
                    { "av", _account.Uid },
                    { "__a", "1" },
                    { "dpr", "1" },
                    { "fb_dtsg", _account.DTSGToken },
                    { "jazoest", "25401" },
                    { "lsd", "_SUj0S8K_WcCtiwQFwev4Y" },
                    { "fb_api_caller_class", "RelayModern" },
                    { "fb_api_req_friendly_name", "FBScrapingWarningMutation" },
                    { "variables", "{}" },
                    { "server_timestamps", "true" },
                    { "doc_id", "6339492849481770" }
                };
                response = await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
                MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
                contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                await response.Content.ReadAsStringAsync();
                response = await client.GetAsync(link);
                MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
                contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                responseStr = await response.Content.ReadAsStringAsync();
            }
            if (response.RequestMessage.RequestUri.AbsolutePath == "/checkpoint/828281030927956/")
            {
                return (StatusJoinBM.Checkpoint956, "Checkpoint 956");
            }
            if (response.RequestMessage.RequestUri.AbsolutePath.Contains("1501092823525282"))
            {
                return (StatusJoinBM.Checkpoint282, "Checkpoint 282");
            }
            string invitationToken = Regex.Match(responseStr, "\\?token=(.*?)&").Groups[1].Value;
            _ = Regex.Match(responseStr, "requestId\":\"(.*?)\"").Groups[1].Value;
            string inviteJoinId = Regex.Match(responseStr, "\"inviteJoinId\":\"(.*?)\"").Groups[1].Value;
            _ = Regex.Match(responseStr, "inviteBusinessName\":\"(.*?)\"").Groups[1].Value;
            string referrerURI = Regex.Match(responseStr, "referrerURI\":\"(.*?)\"").Groups[1].Value.Replace("\\", "");
            if (string.IsNullOrEmpty(referrerURI))
            {
                return (StatusJoinBM.Error, "Block");
            }
            response = await client.GetAsync(referrerURI);
            MediaTypeHeaderValue contentType5 = response.Content.Headers.ContentType;
            contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            responseStr = await response.Content.ReadAsStringAsync();
            string businessId = Regex.Match(responseStr, "{\"businessID\":(.*?),\"").Groups[1].Value.Replace("\"", "");
            if (isCheck)
            {
                StreamWriter sw = new StreamWriter("LinkLive.txt", append: true);
                sw.WriteLine(link + "|" + businessId);
                sw.Close();
                return (StatusJoinBM.Success, businessId);
            }
            string fb_dtsg = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
            Dictionary<string, string> formData = new Dictionary<string, string>
            {
                { "first_name", fName },
                { "last_name", lName },
                { "invitation_token", invitationToken },
                { "receive_marketing_messages", "false" },
                { "user_preferred_business_email", "" },
                { "join_id", inviteJoinId },
                { "mma_qpl_join_id", "" },
                { "__a", "1" },
                { "dpr", "1" },
                { "fb_dtsg", fb_dtsg },
                { "jazoest", "25315" },
                { "lsd", "_SUj0S8K_WcCtiwQFwev4Y" },
                { "qpl_active_flow_ids", "433725819,692797008" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)formData);
            response = await client.PostAsync("https://business.facebook.com/business/invitation/login/", (HttpContent)(object)content);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                MediaTypeHeaderValue contentType6 = response.Content.Headers.ContentType;
                contentType2 = ((contentType6 != null) ? contentType6.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                await response.Content.ReadAsStringAsync();
                return (StatusJoinBM.Error, "Link lỗi");
            }
            MediaTypeHeaderValue contentType7 = response.Content.Headers.ContentType;
            contentType2 = ((contentType7 != null) ? contentType7.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("confirmation") || responseStr.Contains("confirm"))
            {
                string publicKey = "cda78c8ea177c03e8dd655225dca91e3b79eca4ec114f58832234c9b1d2a8578";
                string keyId = "47";
                if (string.IsNullOrEmpty(_account.Password))
                {
                    return (StatusJoinBM.EmptyData, "Không có password");
                }
                string enCryptedPassword = FacebookEncryptHelper.GenerateEncPassword(_account.Password, publicKey, keyId, "5");
                formData.Add("ajax_password", enCryptedPassword);
                formData.Add("confirmed", "1");
                content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)formData);
                response = await client.PostAsync("https://business.facebook.com/business/invitation/login/", (HttpContent)(object)content);
                MediaTypeHeaderValue contentType8 = response.Content.Headers.ContentType;
                contentType2 = ((contentType8 != null) ? contentType8.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                responseStr = await response.Content.ReadAsStringAsync();
                if (responseStr.Contains("828281030927956"))
                {
                    return (StatusJoinBM.Checkpoint956, "Checkpoint 956");
                }
                if (responseStr.Contains("1501092823525282"))
                {
                    return (StatusJoinBM.Checkpoint282, "Checkpoint 282");
                }
            }
            else
            {
                if (responseStr.Contains("error\":1690212"))
                {
                    return (StatusJoinBM.Error, "Link đã chấp nhận");
                }
                if (!responseStr.Contains("payload\":null,\"lid\":"))
                {
                    if (responseStr.Contains("<h1>"))
                    {
                        return (StatusJoinBM.Block, "Block");
                    }
                    if (responseStr.Contains("error\":3252001"))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5.0));
                        return await JoinByLinkInvite(link, fName, lName);
                    }
                    return (StatusJoinBM.Error, responseStr);
                }
                if (responseStr.Contains("\"error\":2446325") || responseStr.Contains("errorSummary\":\""))
                {
                    return (StatusJoinBM.Error, responseStr);
                }
                if (responseStr.Contains("payload\":null,\"lid\":\""))
                {
                    return (StatusJoinBM.Success, "Nhận thành công: " + businessId);
                }
            }
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            exception = ex2;
        }
        return (StatusJoinBM.Error, exception.Message);
    }

    public async Task<bool> KickNutKhangBM(string businessId)
    {
        HttpResponseMessage response = await client.GetAsync("https://business.facebook.com/business-support-home/" + businessId + "/?source=actor_spoke");
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string enforementInstance = Regex.Match(await response.Content.ReadAsStringAsync(), "idesEnforcementInstanceID\":\"(.*?)\"").Groups[1].Value;
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "__a", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "lsd", _account.LSDToken },
            {
                "variables",
                "{\"input\":{\"client_mutation_id\":\"2\",\"actor_id\":\"" + _account.Uid + "\",\"enforcement_instance\":\"" + enforementInstance + "\"}}"
            },
            { "doc_id", "8036119906495815" }
        };
        response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=7500&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
        contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string appelId = Regex.Match(await response.Content.ReadAsStringAsync(), "xfac_appeal_id\":\"(.*?)\"").Groups[1].Value;
        payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "dpr", "1" },
            { "__a", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "lsd", _account.LSDToken },
            {
                "variables",
                "{\"input\":{\"trigger_event_type\":\"XFAC_ACTOR_APPEAL_ENTRY\",\"ufac_design_system\":\"GEODESIC\",\"xfac_id\":\"" + appelId + "\",\"nt_context\":null,\"trigger_session_id\":\"d289e01d-ffc9-43ef-905b-0ee4a5807fd5\"},\"scale\":1}"
            },
            { "doc_id", "29439169672340596" }
        };
        response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=7500&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
        contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        await response.Content.ReadAsStringAsync();
        return true;
    }

    public async Task<string> SubmitImage(string businessId, string appealId, string imageHash)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25862" },
            {
                "lsd",
                string.IsNullOrEmpty(_account.LSDToken) ? "hd22fpiHACBfDIUNkW4Qmn" : _account.LSDToken
            },
            {
                "variables",
                "{\"designSystem\":\"GEODESIC\",\"input\":{\"client_mutation_id\":\"6\",\"actor_id\":\"" + _account.Uid + "\",\"action\":\"UPLOAD_IMAGE\",\"image_upload_handle\":\"" + imageHash + "\",\"caller\":\"BSH_GAME_WEB\",\"enrollment_id\":\"" + appealId + "\"},\"scale\":1}"
            },
            { "doc_id", "24996433536679824" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=2768&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        responseObject["data"]?["ufac_client"]?["id"].ToString();
        return responseObject["data"]?["ufac_client"]?["ui_action"].ToString();
    }

    public async Task<(bool, string)> UploadImage(string imagePath, string businessId)
    {
        string url = "https://rupload.facebook.com/checkpoint_1501092823525282_media_upload/" + Common.CreateRandomString(32) + "&__bid=" + businessId + "&__user=" + _account.Uid + "&__a=1&fb_dtsg=" + _account.DTSGToken;
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        ((HttpHeaders)requestMessage.Headers).Add("sec-fetch-site", "same-origin");
        ((HttpHeaders)requestMessage.Headers).Add("x-entity-length", new FileInfo(imagePath).Length.ToString());
        ((HttpHeaders)requestMessage.Headers).Add("x-entity-name", HttpUtility.UrlEncode(new FileInfo(imagePath).Name));
        ((HttpHeaders)requestMessage.Headers).Add("x-entity-type", "image/jpeg");
        ((HttpHeaders)requestMessage.Headers).Add("offset", "0");
        ((HttpHeaders)requestMessage.Headers).Add("Content-Type", "image/jpeg");
        MultipartFormDataContent form = new MultipartFormDataContent();
        byte[] fileBytes = File.ReadAllBytes(imagePath);
        ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
        ((HttpContent)fileContent).Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        form.Add((HttpContent)(object)fileContent, "upload_0", Path.GetFileName(imagePath));
        requestMessage.Content = (HttpContent)(object)form;
        HttpResponseMessage response = await client.SendAsync(requestMessage);
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseStr))
        {
            return (false, null);
        }
        string hValue = Regex.Match(responseStr, "\"h\":\"(.*?)\"").Groups[1].Value;
        return (true, hValue);
    }

    public async Task<string> UploadVerifyCode(string businessId, string appealId, string verifyCode)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25862" },
            {
                "lsd",
                string.IsNullOrEmpty(_account.LSDToken) ? "hd22fpiHACBfDIUNkW4Qmn" : _account.LSDToken
            },
            {
                "variables",
                "{\"designSystem\":\"GEODESIC\",\"input\":{\"client_mutation_id\":\"5\",\"actor_id\":\"" + _account.Uid + "\",\"action\":\"SUBMIT_CODE\",\"code\":\"" + verifyCode + "\",\"caller\":\"BSH_GAME_WEB\",\"enrollment_id\":\"" + appealId + "\"},\"scale\":1}"
            },
            { "doc_id", "24996433536679824" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=2768&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        responseObject["data"]?["ufac_client"]?["id"].ToString();
        return responseObject["data"]?["ufac_client"]?["state_hash"].ToString();
    }

    public async Task<string> UploadPhoneNumber(string businessId, string appealId, string phoneNumber)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25862" },
            {
                "lsd",
                string.IsNullOrEmpty(_account.LSDToken) ? "hd22fpiHACBfDIUNkW4Qmn" : _account.LSDToken
            },
            {
                "variables",
                "{\"designSystem\":\"GEODESIC\",\"input\":{\"client_mutation_id\":\"4\",\"actor_id\":\"" + _account.Uid + "\",\"action\":\"SET_CONTACT_POINT_WA\",\"contactpoint\":\"" + phoneNumber + "\",\"country_code\":\"VN\",\"caller\":\"BSH_GAME_WEB\",\"enrollment_id\":\"" + appealId + "\"},\"scale\":1}"
            },
            { "doc_id", "24996433536679824" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=2768&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        responseObject["data"]?["ufac_client"]?["id"].ToString();
        return responseObject["data"]?["ufac_client"]?["state_hash"].ToString();
    }

    public async Task<string> ValidUploadPhoneStep(string businessId, string appealId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25862" },
            {
                "lsd",
                string.IsNullOrEmpty(_account.LSDToken) ? "hd22fpiHACBfDIUNkW4Qmn" : _account.LSDToken
            },
            {
                "variables",
                "{\"designSystem\":\"GEODESIC\",\"input\":{\"client_mutation_id\":\"3\",\"actor_id\":\"" + _account.Uid + "\",\"action\":\"SUBMIT_GENERIC_CHALLENGE_CHOOSER_OPTION\",\"option_key\":\"idv\",\"caller\":\"BSH_GAME_WEB\",\"enrollment_id\":\"" + appealId + "\"},\"scale\":1}"
            },
            { "doc_id", "24996433536679824" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=2768&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        responseObject["data"]?["ufac_client"]?["id"].ToString();
        return responseObject["data"]?["ufac_client"]?["state_hash"].ToString();
    }

    public async Task<(bool, string)> ResolveCaptcha(string businessId, string appealId, string captchaPersist, string gCaptchaResponse)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25862" },
            {
                "lsd",
                string.IsNullOrEmpty(_account.LSDToken) ? "hd22fpiHACBfDIUNkW4Qmn" : _account.LSDToken
            },
            {
                "variables",
                "{\"designSystem\":\"GEODESIC\",\"input\":{\"client_mutation_id\":\"3\",\"actor_id\":\"" + _account.Uid + "\",\"action\":\"SUBMIT_BOT_CAPTCHA_RESPONSE\",\"bot_captcha_persist_data\":\"" + captchaPersist + "\",\"bot_captcha_response\":\"" + gCaptchaResponse + "\",\"caller\":\"BSH_GAME_WEB\",\"enrollment_id\":\"" + appealId + "\"},\"scale\":1}"
            },
            { "doc_id", "24996433536679824" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=2768&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        responseObject["data"]?["ufac_client"]?["id"].ToString();
        string stateHash = responseObject["data"]?["ufac_client"]?["state_hash"].ToString();
        if (stateHash == "bot_captcha_challenge_ui_state")
        {
            return (false, "Captcha");
        }
        return (true, "");
    }

    public async Task<(bool, string)> GetCaptchaPersist(string businessId, string appealId)
    {
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25862" },
            {
                "lsd",
                string.IsNullOrEmpty(_account.LSDToken) ? "hd22fpiHACBfDIUNkW4Qmn" : _account.LSDToken
            },
            {
                "variables",
                "{\"designSystem\":\"GEODESIC\",\"enrollmentID\":\"" + appealId + "\",\"scale\":1}"
            },
            { "doc_id", "25040410668989121" }
        };
        HttpResponseMessage response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=2768&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        responseObject["data"]?["ufac_client"]?["id"].ToString();
        string stateHash = responseObject["data"]?["ufac_client"]?["state_hash"].ToString();
        if (string.IsNullOrEmpty(stateHash))
        {
            return (false, "Empty StateHash");
        }
        if (stateHash == "bot_captcha_challenge_ui_state")
        {
            string captchaPersist = responseObject["data"]?["ufac_client"]?["state"]?["captcha_persist_data"].ToString();
            if (string.IsNullOrEmpty(captchaPersist))
            {
                return (false, "Can't found captchaPersist");
            }
            return (true, captchaPersist);
        }
        return (true, "Captcha Solve");
    }

    public async Task<string> GetAppealId(string businessId)
    {
        HttpResponseMessage response = await client.GetAsync("https://business.facebook.com/business-support-home/" + businessId + "/?source=actor_spoke");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string enforementInstance = Regex.Match(await response.Content.ReadAsStringAsync(), "idesEnforcementInstanceID\":\"(.*?)\"").Groups[1].Value;
        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "av", _account.Uid },
            { "__bid", businessId },
            { "__user", _account.Uid },
            { "__a", "1" },
            { "dpr", "1" },
            { "fb_dtsg", _account.DTSGToken },
            { "jazoest", "25862" },
            {
                "lsd",
                string.IsNullOrEmpty(_account.LSDToken) ? "hd22fpiHACBfDIUNkW4Qmn" : _account.LSDToken
            },
            {
                "variables",
                "{\"input\":{\"client_mutation_id\":\"2\",\"actor_id\":\"" + _account.Uid + "\",\"enforcement_instance\":\"" + enforementInstance + "\"}}"
            },
            { "doc_id", "29485441471102331" }
        };
        response = await client.PostAsync("https://business.facebook.com/api/graphql/?_callFlowletID=0&_triggerFlowletID=2768&qpl_active_e2e_trace_ids=", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
        contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        string xfacAppealId = responseObject["data"]?["xfb_XFACGraphQLAppealManagerFetchOrCreateAppeal"]?["xfac_appeal_id"]?.ToString();
        if (string.IsNullOrEmpty(xfacAppealId))
        {
            return null;
        }
        return xfacAppealId;
    }

    public async Task<bool> LoginBusinessFacebook()
    {
        try
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://mobile.facebook.com/login.php?");
            ((HttpHeaders)requestMessage.Headers).Add("User-Agent", "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_5 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Mobile/8L1");
            ((HttpHeaders)requestMessage.Headers).Add("Cookie", _account.Cookie);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            string contentType2 = ((contentType != null) ? contentType.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            await response.Content.ReadAsStringAsync();
            int retry = 3;
            string lsd;
            string userNonce;
            string dtsgToken;
            string urlSuit;
            do
            {
                response = await client.GetAsync("https://business.facebook.com/business/loginpage/?option=IG&from_ig_multi_admin_invite=false&is_ig_oidc_with_redirect=false&cma_account_switch=false&is_ig_switching_account=false&next=https%3A%2F%2Fbusiness.facebook.com%2F%3Fnav_ref%3Dbiz_unified_f3_login_page_to_mbs%26biz_login_source%3Dbiz_unified_f3_ig_oidc_pc_login_button");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
                contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                string responseStr = await response.Content.ReadAsStringAsync();
                _ = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                lsd = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                userNonce = Regex.Match(responseStr, "user_nonce%22%3A%22(.*?)%").Groups[1].Value;
                _ = Regex.Match(responseStr, "page_uri:\"(.*?)\"").Groups[1].Value;
                string url = response.RequestMessage.RequestUri.OriginalString;
                if (url.Contains("/cookie/consent/"))
                {
                    Dictionary<string, string> formData = new Dictionary<string, string>
                    {
                        { "accept_only_essential", "false" },
                        { "consent_to_everything", "true" },
                        { "__aaid", "0" },
                        { "__user", "0" },
                        { "__a", "1" },
                        { "__req", "3" },
                        { "__hs", "20337.BP:DEFAULT.2.0...0" },
                        { "dpr", "1" },
                        { "__ccg", "EXCELLENT" },
                        { "__rev", "1026800727" },
                        { "__s", "qmu37q:lew5az:q97bd9" },
                        { "__hsi", "7547001844489887747" },
                        { "__dyn", "7xe6E5aQ5E5ObwKBAg5S1Dxu13wqovzEdEc8uw9-3K0lW4o2vw6_CwjE1EE2Cwooa86u0nS4o5-0jx0Fwqo5W1yw9O482HK0JUeo2swaS1HwywnE0Caaw4kwbS1Lw60wr831w4xwtU5K0UE1iU" },
                        { "__hsdp", "8dE9iNQQLaifGh2N343t1Ol2HhsCSC1xEFVyAq6yAcDCxy7E_yZ0qE5652d2FiKGrCg9l4yQ2Z02aU0Xx04yAx6" },
                        { "__hblp", "0Uway1UwMw5OwbS16w77w4Rw2eo5-3i0pu09bwfu05DoW03Sm0eCw9C0om0bruczo7O3vgoFp8y0aUw2l812E1zU3dw" },
                        { "lsd", lsd },
                        { "jazoest", "2978" },
                        { "__spin_r", "1026800727" },
                        { "__spin_b", "trunk" },
                        { "__spin_t", "1757173297" },
                        { "__jssesw", "1" },
                        { "qpl_active_flow_ids", "373047013" }
                    };
                    await client.PostAsync("https://business.facebook.com/cookie/consent/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)formData));
                }
                else if (url.Contains("/latest/home?asset_id="))
                {
                    return true;
                }
                response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
                contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                responseStr = await response.Content.ReadAsStringAsync();
                dtsgToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
                _account.Uid = Regex.Match(responseStr, "USER_ID\":\"(.*?)\"").Groups[1].Value;
                if (string.IsNullOrEmpty(userNonce))
                {
                    userNonce = Regex.Match(responseStr, "user_nonce%22%3A%22(.*?)%").Groups[1].Value;
                }
                if (string.IsNullOrEmpty(userNonce))
                {
                    return false;
                }
                response = await client.GetAsync("https://business.facebook.com/latest/?nav_ref=biz_unified_f3_login_page_to_mbs");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                MediaTypeHeaderValue contentType5 = response.Content.Headers.ContentType;
                contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
                if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
                {
                    response.Content.Headers.ContentType.CharSet = "utf-8";
                }
                await response.Content.ReadAsStringAsync();
                urlSuit = response.RequestMessage.RequestUri.OriginalString;
                if (!urlSuit.Contains("https://business.facebook.com/business/loginpage/"))
                {
                    break;
                }
                retry--;
            }
            while (retry > 0);
            if (urlSuit.Contains("business_id"))
            {
                return true;
            }
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "av", _account.Uid },
                { "fb_dtsg", dtsgToken },
                { "jazoest", "26378" },
                { "lsd", lsd },
                { "fb_api_caller_class", "RelayModern" },
                { "fb_api_req_friendly_name", "PolarisOidcAuthorizePageConsentToOIDCAuthMutation" },
                {
                    "variables",
                    "{\"input\":{\"client_mutation_id\":\"1\",\"actor_id\":\"" + _account.Uid + "\",\"flow_id\":\"5b5c2edf-8d20-4795-bca8-a144679ca2f1\",\"platform_app_id\":\"532380490911317\",\"redirect_uri\":\"https://business.facebook.com/business/loginpage/igoidc/callback/idtoken/\",\"scope\":\"openid\",\"state\":\"{\\\"user_nonce\\\":\\\"" + userNonce + "\\\",\\\"from_ig_login_upsell_sso\\\":null,\\\"login_source\\\":\\\"fbs_web_landing_page\\\",\\\"next\\\":\\\"\\\\u00252F\\\\u00253Fnav_ref\\\\u00253Dbizweb_landing_ig_login_button\\\\u002526biz_login_source\\\\u00253Dbizweb_landing_login_ig_oidc_w_pc_login_button\\\",\\\"require_professional\\\":true,\\\"create_business_manager\\\":true}\"}}"
                },
                { "server_timestamps", "true" },
                { "doc_id", "6852193194858773" }
            };
            response = await client.PostAsync("https://www.instagram.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            MediaTypeHeaderValue contentType6 = response.Content.Headers.ContentType;
            contentType2 = ((contentType6 != null) ? contentType6.CharSet : null);
            if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }
            string redirectUrl = JObject.Parse(await response.Content.ReadAsStringAsync())["data"]?["xig_ig_oidc_auth_user_consent"]?["redirect_uri"]?.ToString();
            if (string.IsNullOrEmpty(redirectUrl))
            {
                return false;
            }
            if (!(await client.GetAsync(redirectUrl)).IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<StatusCookie> CheckCookieStatusIG()
    {
        HttpResponseMessage response = await client.GetAsync("https://www.instagram.com/");
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        string cookie = cookieContainer.CookieToString("https://www.instagram.com").Result;
        _ = Regex.Match(cookie, "csrftoken=(.*?);").Groups[1].Value;
        _ = Regex.Match(responseStr, "NON_FACEBOOK_USER_ID\":\"(.*?)\"").Groups[1].Value;
        _ = Regex.Match(responseStr, "username\":\"(.*?)\"").Groups[1].Value;
        if (!responseStr.Contains(_account.Uid) || !cookie.Contains("ds_user_id"))
        {
            return StatusCookie.Die;
        }
        return StatusCookie.Live;
    }

    public async Task<StatusCookie> LoginInstagramUidPass()
    {
        HttpResponseMessage response = await client.GetAsync("https://www.instagram.com/");
        MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
        string contentType2 = ((contentType != null) ? contentType.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string responseStr = await response.Content.ReadAsStringAsync();
        string jazoest = (string.IsNullOrEmpty(Regex.Match(responseStr, "jazoest=(\\d+)\"").Groups[1].Value) ? Regex.Match(responseStr, "jazoest\",\"value\":\"(.*?)\"").Groups[1].Value : Regex.Match(responseStr, "jazoest=(\\d+)\"").Groups[1].Value);
        response = await client.GetAsync("https://www.instagram.com/data/shared_data/");
        MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
        contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        string csrfToken = Regex.Match(await response.Content.ReadAsStringAsync(), "csrf_token\":\"(.*?)\"").Groups[1].Value;
        string encPassword = FacebookEncryptHelper.GenerateEncPassword(_account.Password, "dfad72b100e79879568de06d5b09768ebc643eba293f1e5b5572d71dbffca637", "47", "10");
        encPassword.Replace("#PWD_BROWSER", "#PWD_INSTAGRAM_BROWSER");
        List<KeyValuePair<string, string>> payload = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("enc_password", $"#PWD_INSTAGRAM_BROWSER:0:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}:{_account.Password}"),
            new KeyValuePair<string, string>("caaF2DebugGroup", "0"),
            new KeyValuePair<string, string>("isPrivacyPortalReq", "false"),
            new KeyValuePair<string, string>("loginAttemptSubmissionCount", "0"),
            new KeyValuePair<string, string>("optIntoOneTap", "false"),
            new KeyValuePair<string, string>("queryParams", "{}"),
            new KeyValuePair<string, string>("trustedDeviceRecords", "{}"),
            new KeyValuePair<string, string>("username", _account.Uid),
            new KeyValuePair<string, string>("jazoest", jazoest)
        };
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.instagram.com/api/v1/web/accounts/login/ajax/");
        ((HttpHeaders)requestMessage.Headers).Add("x-requested-with", "XMLHttpRequest");
        ((HttpHeaders)requestMessage.Headers).Add("x-csrftoken", csrfToken);
        requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload);
        MediaTypeHeaderValue contentType4 = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        contentType4.CharSet = "UTF-8";
        requestMessage.Content.Headers.ContentType = contentType4;
        response = await client.SendAsync(requestMessage);
        MediaTypeHeaderValue contentType5 = response.Content.Headers.ContentType;
        contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
        if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
        {
            response.Content.Headers.ContentType.CharSet = "utf-8";
        }
        responseStr = await response.Content.ReadAsStringAsync();
        if (responseStr.Contains("\"lock\":true"))
        {
            return StatusCookie.Checkpoint282;
        }
        if (responseStr.Contains("two_factor_required\":true"))
        {
            if (string.IsNullOrEmpty(_account.Key2FA))
            {
                return StatusCookie.Error2FA;
            }
            string code = Common.GetCode(_account.Key2FA);
            string indentifier = JObject.Parse(responseStr)["two_factor_info"]?["two_factor_identifier"]?.ToString();
            if (string.IsNullOrEmpty(indentifier))
            {
                return StatusCookie.Error;
            }
            Dictionary<string, string> payloadVerifyCode = new Dictionary<string, string>
            {
                { "identifier", indentifier },
                { "isPrivacyPortalReq", "false" },
                { "queryParams", "{}" },
                { "trust_signal", "true" },
                { "username", _account.Uid },
                { "verification_method", "3" },
                { "verificationCode", code },
                { "jazoest", jazoest }
            };
            requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.instagram.com/api/v1/web/accounts/login/ajax/two_factor/");
            ((HttpHeaders)requestMessage.Headers).Add("x-requested-with", "XMLHttpRequest");
            ((HttpHeaders)requestMessage.Headers).Add("x-csrftoken", csrfToken);
            requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payloadVerifyCode);
            contentType4 = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            contentType4.CharSet = "UTF-8";
            requestMessage.Content.Headers.ContentType = contentType4;
            if ((await (await client.SendAsync(requestMessage)).Content.ReadAsStringAsync()).Contains("\"authenticated\":true"))
            {
                return StatusCookie.Live;
            }
        }
        string cookie = cookieContainer.CookieToString("https://www.instagram.com").Result;
        if (!cookie.Contains("ds_user"))
        {
            return StatusCookie.Die;
        }
        return StatusCookie.Live;
    }

    public async Task<StatusCookie> LoginInstagram(int typeLogin)
    {
        InitializeHttpClient();
        return typeLogin switch
        {
            0 => await CheckCookieStatusIG(),
            1 => await LoginInstagramUidPass(),
            _ => throw new NotImplementedException(),
        };
    }

    private void InitializeHttpClient()
    {
        (client, cookieContainer) = HttpClientCommon.CreateHttpClient(_account.Cookie, useragent: _userAgent, proxyStr: _proxy, domain: "instagram.com");
    }

    public string GetCookie()
    {
        return cookieContainer.CookieToString().Result;
    }
}
