using System;
using Thorm;

namespace Simple.Schema
{
    public class User : IModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }
    }
}
