﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace InventoryManagementWeb.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? ProductId { get; set; }

    public int TransactionType { get; set; } // 1 kalo berhasil 0 kalo gagal

    public int Quantity { get; set; }

    public DateTime? Date { get; set; }

    public Product Product { get; set; }
    
}