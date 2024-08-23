﻿using SaleService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleService.Repositories.InvoiceRepository
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly SaleContext _context;
        private readonly ILogger<InvoiceRepo> _logger;

        public InvoiceRepo(SaleContext context, ILogger<InvoiceRepo> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoiceAsync()
        {
            return await _context.Invoices.ToListAsync();
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _context.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId);
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice));
            }

            await _context.Invoices.AddAsync(invoice);
            await SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice));
            }

            _context.Invoices.Update(invoice);
            await SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByCustomerId(int customerId)
        {
            var query = await _context.Invoices
                .Where(i => i.Id == customerId)
                .ToListAsync();
            return query;
        }
    }
}
