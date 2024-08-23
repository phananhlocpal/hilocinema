using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly string _vnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private readonly string _merchantId = "MDNUWEVW";
        private readonly string _secretKey = "60ED1DZEMBLRXS8H8YNGPOZTLFQXTYUM";

        [HttpPost("createPayment")]
        public IActionResult CreatePayment([FromBody] Invoice invoice)
        {
            if (!invoice.Total.HasValue)
            {
                return BadRequest("Total amount is required");
            }

            var vnpayRequest = new VnPayRequest
            {
                Amount = (long)(invoice.Total.Value * 100), 
                OrderId = invoice.CustomerId.ToString(),
                ReturnUrl = "http://localhost:2000/payment/return",
                VnpHashSecret = _secretKey,
                VnpMerchantId = _merchantId
            };

            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var vnpayUrl = vnpayRequest.GenerateUrl(_vnpUrl, clientIp);
            return Redirect(vnpayUrl);
        }

        [HttpGet("return")]
        public IActionResult PaymentReturn()
        {
            var query = Request.Query;
            var responseCode = query["vnp_ResponseCode"];
            var orderId = query["vnp_TxnRef"];
            // Validate the response and update your system

            return Redirect("http://localhost:2000/");
        }
    }

    public class VnPayRequest
    {
        public long Amount { get; set; }
        public string OrderId { get; set; }
        public string ReturnUrl { get; set; }
        public string VnpHashSecret { get; set; }
        public string VnpMerchantId { get; set; }

        public string GenerateUrl(string baseUrl, string clientIp)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "vnp_Version", Uri.EscapeDataString("2.0.0") },
                { "vnp_Command", Uri.EscapeDataString("pay") },
                { "vnp_TmnCode", Uri.EscapeDataString(VnpMerchantId) },
                { "vnp_Amount", Uri.EscapeDataString(Amount.ToString()) },
                { "vnp_CurrCode", Uri.EscapeDataString("VND") },
                { "vnp_TxnRef", Uri.EscapeDataString(OrderId) },
                { "vnp_OrderInfo", Uri.EscapeDataString("Thanh toán đơn hàng") },
                { "vnp_ReturnUrl", Uri.EscapeDataString(ReturnUrl) },
                { "vnp_IpAddr", Uri.EscapeDataString(clientIp) },
                { "vnp_CreateDate", Uri.EscapeDataString(DateTime.Now.ToString("yyyyMMddHHmmss")) },
            };

            var sortedQueryParams = queryParams.OrderBy(x => x.Key).ToList();
            var hashData = string.Join("&", sortedQueryParams.Select(x => $"{x.Key}={x.Value}"));
            var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(VnpHashSecret));
            var hash = BitConverter.ToString(hmac.ComputeHash(Encoding.ASCII.GetBytes(hashData))).Replace("-", "").ToUpper();

            return $"{baseUrl}?{hashData}&vnp_SecureHash={hash}";
        }

    }
}
