using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ResourcePackerGUI.Application.Definitions.Queries;

namespace ResourcePackerGUI.Application.Definitions.Handlers
{
    public class ExportDefinitionsQueryHandler : IRequestHandler<ExportDefinitionsQuery>
    {
        //public Task<Unit> Handle(ExportDefinitionsQuery request, CancellationToken cancellationToken)
        //{
        //    var percentage = 0;
        //    using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
        //    progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
        //    progressTimer.Enabled = request.Progress != null;

        //    for (var i = 0; i < items.Count; i++)
        //    {
        //    }
        //}
        public Task<Unit> Handle(ExportDefinitionsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
