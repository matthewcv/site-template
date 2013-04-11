using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;

namespace mywebsite.backend.Service
{
    public interface ICandyService
    {
        
    }
    public class CandyService:ICandyService
    {
        public CandyService(IDocumentSession session)
        {
            
        }
    }
}
