using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ITokenGeneratorService
    {
        Application.DTOs.Token CreateAccesToken(int second, Customer customer, IList<string> userRoles);
    }
}
