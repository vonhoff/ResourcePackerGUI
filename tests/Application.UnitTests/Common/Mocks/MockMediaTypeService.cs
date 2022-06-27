using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Domain.Entities;

namespace Application.UnitTests.Common.Mocks
{
    internal class MockMediaTypeService : IMediaTypeService
    {
        public MediaType? GetTypeByData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public MediaType? GetTypeByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
