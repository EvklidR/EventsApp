using AuthorisationService.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorisationService.Application.Interfaces.UseCases
{
    public interface ICheckUserById
    {
        Task<bool> ExecuteAsync(int id);
    }
}
