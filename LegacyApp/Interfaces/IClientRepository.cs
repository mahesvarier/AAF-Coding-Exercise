using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyApp.Models;

namespace LegacyApp.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int id);
    }
}
