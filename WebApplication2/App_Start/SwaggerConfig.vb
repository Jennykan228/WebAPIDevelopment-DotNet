Imports System.Web.Http
Imports WebActivatorEx
Imports Swashbuckle.Application

<Assembly: PreApplicationStartMethod(GetType(WebApplication2.SwaggerConfig), "Register")>

Namespace WebApplication2
    Public Class SwaggerConfig
        Public Shared Sub Register()
            Dim thisAssembly = GetType(SwaggerConfig).Assembly

            GlobalConfiguration.Configuration _
          .EnableSwagger(Sub(c)
                             ' Use "SingleApiVersion" to describe a single version API. Swagger 2.0 includes an "Info" object to
                             ' hold additional metadata for an API. Version and title are required but you can also provide
                             ' additional fields by chaining methods off SingleApiVersion.
                             c.SingleApiVersion("v1", "WebApplication2")

                             ' If you annotate Controllers and API Types with
                             ' Xml comments, you can incorporate those comments into the generated docs and UI. 
                             ' You can enable this by providing the path to one or more Xml comment files.
                             'c.IncludeXmlComments(GetXmlCommentsPath())

                         End Sub) _
       .EnableSwaggerUi(Sub(c)
                            ' Use the "DocumentTitle" option to change the Document title.
                            ' Very helpful when you have multiple Swagger pages open, to tell them apart.
                            'c.DocumentTitle("My Swagger UI")

                            ' Use this option to control how the Operation listing is displayed.
                            ' It can be set to "None" (default), "List" (shows operations for each resource),
                            ' or "Full" (fully expanded: shows operations and their details).
                            'c.DocExpansion(DocExpansion.List)

                        End Sub)
        End Sub
    End Class
End Namespace