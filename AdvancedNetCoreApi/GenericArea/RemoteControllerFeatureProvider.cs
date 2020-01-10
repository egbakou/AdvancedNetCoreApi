using AdvancedNetCoreApi.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace AdvancedNetCoreApi.GenericArea
{
    public class RemoteControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var remoteCode = new HttpClient().GetStringAsync("https://gist.githubusercontent.com/egbakou/857e89fc804fea44254c102bf7f7e5e0/raw/83ac0eeac860df73a44c9903d7880a937d95b866/AdancedEFModes.txt").GetAwaiter().GetResult();
            //var remoteCode = new HttpClient().GetStringAsync("https://localhost:44397/css/poco.txt").GetAwaiter().GetResult();
            if (remoteCode != null)
            {
                var currentAssembly =Assembly.GetExecutingAssembly().GetName().Name;
                var compilation = CSharpCompilation.Create(currentAssembly, new[] { CSharpSyntaxTree.ParseText(remoteCode) },
                    new[] {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(RemoteControllerFeatureProvider).Assembly.Location)
                    },
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                using (var ms = new MemoryStream())
                {
                    var emitResult = compilation.Emit(ms);

                    if (!emitResult.Success)
                    {
                        // handle, log errors etc
                        Debug.WriteLine("Compilation failed!");
                        return;
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    var candidates = assembly.GetExportedTypes();

                    try
                    {
                        foreach (var candidate in candidates)
                        {
                            if (!candidate.IsAbstract)
                                feature.Controllers.Add(typeof(BaseController<>).MakeGenericType(candidate).GetTypeInfo());
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.InnerException.Message);
                    }
                    
                }
            }
        }
    }
}
