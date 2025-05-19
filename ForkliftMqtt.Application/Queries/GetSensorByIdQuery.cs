using ForkliftMqtt.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.Queries
{
    public class GetSensorByIdQuery : IRequest<ForkliftSensorDto>
    {
        public string Id { get; set; }
    }

}
