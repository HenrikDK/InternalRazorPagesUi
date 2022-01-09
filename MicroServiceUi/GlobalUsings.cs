global using System;
global using System.Diagnostics;
global using System.Linq;
global using System.Net;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Transactions;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Html;
global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using Lamar;
global using Lamar.Microsoft.DependencyInjection;
global using Prometheus;
global using Serilog;
global using Serilog.Formatting.Json;