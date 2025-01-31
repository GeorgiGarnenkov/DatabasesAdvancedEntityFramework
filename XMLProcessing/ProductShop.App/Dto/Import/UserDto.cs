﻿namespace ProductShop.App.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserDto
    {
        [XmlAttribute("firstName")]
        public string FirstName { get; set; }

        [XmlAttribute("lastName")]
        [MinLength(3)]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }
    }
}