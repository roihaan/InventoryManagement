﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace InventoryManagementWeb.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; }

    public int StockLevel { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}