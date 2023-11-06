using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using Newtonsoft.Json.Linq;

public class kintoneAPIHundler : MonoBehaviour
{

    public static void sendKintoneAPI(string USER, string MAIL, string API_KEY, string API_URL, string projectName, string todoContent)
    {
        var parameters = new Dictionary<string, object>()
        {
            { "app", 1 },
            {
                "record", new Dictionary<string, object>()
                {
                    {"str_projectName", new Dictionary<string, string>()
                        {
                            {"value", projectName}
                        } 
                    },
                    {"str_toDo", new Dictionary<string, string>()
                        {
                            {"value", todoContent}
                        } 
                    },
                    {"更新者", new Dictionary<string, object>()
                        {
                            {"type", "MODIFIER"},
                            {"value", new Dictionary<string, string>()
                                {
                                    {"code", MAIL},
                                    {"name", USER}
                                }
                            }
                        }
                    },
                    {"作成者", new Dictionary<string, object>()
                        {
                            {"type", "CREATOR"},
                            {"value", new Dictionary<string, string>()
                                {
                                    {"code", MAIL},
                                    {"name", USER}
                                }
                            }
                        }
                    }
                }
            },
            
        };
        using (var httpClient = new HttpClient())
        {
            string json = JsonConvert.SerializeObject(parameters);
            
            var content  = new StringContent(json, Encoding.UTF8, "application/json");
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //Debug.Log("apikey:" + API_KEY);
            //Debug.Log("apiurl:" + API_URL);
            //Debug.Log(json);
            httpClient.DefaultRequestHeaders.Add(@"X-Cybozu-API-Token", API_KEY);
            var response = httpClient.PostAsync(API_URL,content).Result;
            
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(responseBody);
            Debug.Log(jsonResponse.ToString());
        }
    }
}
