using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MTALUAC
{
    public class LUACAPI
    {
        public static async Task<bool> CompileFile(string filename)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "http://luac.mtasa.com/"))
                {
                    var fileName = Path.GetFileName(filename);
                    var fileBytes = File.ReadAllBytes(filename);

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new StringContent("1"), "compile");
                    multipartContent.Add(new StringContent("0"), "debug");
                    multipartContent.Add(new StringContent("3"), "obfuscate");
                    multipartContent.Add(new ByteArrayContent(fileBytes), "luasource", fileName);
                    request.Content = multipartContent;

                    var response = await httpClient.SendAsync(request);

                    var content = await response.Content.ReadAsByteArrayAsync();

                    var compiledFile = filename.Replace(".lua", ".luac");
                    if (File.Exists(compiledFile))
                        File.Delete(compiledFile);

                    File.WriteAllBytes(compiledFile, content);

                    return true;
                }
            }
        }
    }
}
