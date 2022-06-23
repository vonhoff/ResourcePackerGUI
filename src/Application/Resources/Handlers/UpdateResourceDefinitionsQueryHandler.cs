using MediatR;
using ResourcePackerGUI.Application.Resources.Queries;

namespace ResourcePackerGUI.Application.Resources.Handlers
{
    public class UpdateResourceDefinitionsQueryHandler : IRequestHandler<UpdateResourceDefinitionsQuery>
    {
        public Task<Unit> Handle(UpdateResourceDefinitionsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}