using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;


namespace TeduShop.Web.Infrastructure.NganLuongAPI
{
    public class APICheckoutV3
    {

        public ResponseInfo GetUrlCheckout(RequestInfo requestContent, string payment_method = "NL")
        {

            requestContent.Payment_method = payment_method;
            String requestinfo = GetParamPost(requestContent);
            String result = HttpPost(requestinfo);
            result = result.Replace("&", "&amp;");
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(result);
            XmlNodeList root = dom.DocumentElement.ChildNodes;

            ResponseInfo resResult = new ResponseInfo();
            resResult.Checkout_url = root.Item(4).InnerText;
            resResult.Description = root.Item(2).InnerText;
            resResult.Error_code = root.Item(0).InnerText;
            resResult.Token = root.Item(1).InnerText;

            return resResult;
        }

        public ResponseCheckOrder GetTransactionDetail(RequestCheckOrder info)
        {


            String request = "";
            request += "function=" + info.Funtion;
            request += "&version=" + info.Version;
            request += "&merchant_id=" + info.Merchant_id;
            request += "&merchant_password=" + CreateMD5Hash(info.Merchant_password);
            request += "&token=" + info.Token;
            String result = HttpPost(request);
            result = result.Replace("&", "&amp;");
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(result);
            XmlNodeList root = dom.DocumentElement.ChildNodes;

            ResponseCheckOrder objResult = new ResponseCheckOrder();


            objResult.errorCode = root.Item(0).InnerText;
            objResult.token = root.Item(1).InnerText;
            objResult.description = root.Item(2).InnerText;
            objResult.transactionStatus = root.Item(3).InnerText;
            //objResult.receiver_email = root.Item(4).InnerText; //receiver_email
            objResult.order_code = root.Item(5).InnerText;
            objResult.paymentAmount = root.Item(6).InnerText; //total_amount
            //objResult.payment_method = root.Item(7).InnerText; //payment_method
            //objResult.bank_code = root.Item(8).InnerText; //bank_code
            //objResult.payment_type = root.Item(9).InnerText; //payment_type
            //objResult.description = root.Item(10).InnerText; //order_description
            //objResult.tax_amount = root.Item(11).InnerText; //tax_amount
            //objResult.discount_amount = root.Item(12).InnerText; //discount_amount
            //objResult.fee_shipping = root.Item(13).InnerText; //fee_shipping
            //objResult.return_url = root.Item(14).InnerText; //return_url
            // objResult.cancel_url = root.Item(15).InnerText; //cancel_url
            objResult.payerName = root.Item(16).InnerText; //buyer_fullname
            objResult.payerEmail = root.Item(17).InnerText; //buyer_email
            objResult.payerMobile = root.Item(18).InnerText; //buyer_mobile
            //objResult.buyer_address = root.Item(19).InnerText; //buyer_address
            //objResult.affiliate_code = root.Item(20).InnerText; //affiliate_code
            objResult.transactionId = root.Item(21).InnerText;


            objResult.errorCode = root.Item(0).InnerText;
            objResult.token = root.Item(1).InnerText;
            objResult.description = root.Item(2).InnerText;
            objResult.transactionStatus = root.Item(3).InnerText;
            objResult.order_code = root.Item(5).InnerText;
            objResult.paymentAmount = root.Item(6).InnerText;
            objResult.transactionId = root.Item(21).InnerText;

            /*
            String error_code =root.Item(0).InnerText;
            String token =root.Item(1).InnerText;
            String description =root.Item(2).InnerText;
            String transaction_status =root.Item(3).InnerText;
            String receiver_email =root.Item(4).InnerText;
            String order_code =root.Item(5).InnerText;
            String total_amount =root.Item(6).InnerText;
            String payment_method =root.Item(7).InnerText;
            String bank_code =root.Item(8).InnerText;
            String payment_type =root.Item(9).InnerText;
            String order_description =root.Item(10).InnerText;
            String tax_amount =root.Item(11).InnerText;
            String discount_amount =root.Item(12).InnerText;
            String fee_shipping =root.Item(13).InnerText;
            String return_url =root.Item(14).InnerText;
            String cancel_url =root.Item(15).InnerText;
            String buyer_fullname =root.Item(16).InnerText;            
            String buyer_email =root.Item(17).InnerText;
            String buyer_mobile =root.Item(18).InnerText;
            String buyer_address =root.Item(19).InnerText;
            String affiliate_code =root.Item(20).InnerText;
            String transaction_id =root.Item(21).InnerText;
         */
            return objResult;
        }

        private static String GetParamPost(RequestInfo info)
        {

            String request = "";

            request += "function=" + info.Funtion;
            request += "&cur_code=" + info.cur_code;
            request += "&version=" + info.Version;
            request += "&merchant_id=" + info.Merchant_id;
            request += "&receiver_email=" + info.Receiver_email;
            request += "&merchant_password=" + CreateMD5Hash(info.Merchant_password);
            request += "&order_code=" + info.Order_code;
            request += "&total_amount=" + info.Total_amount;
            request += "&payment_method=" + info.Payment_method;
            request += "&bank_code=" + info.bank_code;
            request += "&payment_type=" + info.Payment_type;
            request += "&order_description=" + info.order_description;
            request += "&tax_amount=" + info.tax_amount;
            request += "&fee_shipping=" + info.fee_shipping;
            request += "&discount_amount=" + info.Discount_amount;
            request += "&return_url=" + info.return_url;
            request += "&cancel_url=" + info.cancel_url;
            request += "&buyer_fullname=" + info.Buyer_fullname;
            request += "&buyer_email=" + info.Buyer_email;
            request += "&buyer_mobile=" + info.Buyer_mobile;

            return request;
        }


        private static String HttpPost(string postData)
        {

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);

            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://www.nganluong.vn/checkout.api.nganluong.post.php");
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.

            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string output = reader.ReadToEnd();
            response.Close();
            return output;
        }

        private static String CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private static String GetErrorMessage(string _ErrorCode)
        {
            String _Message = "";
            switch (_ErrorCode)
            {
                case "00":
                    _Message = "Giao dịch thành công";
                    break;
                case "01":
                    _Message = "Lỗi, địa chỉ IP truy cập API của NgânLượng.vn bị từ chối";
                    break;
                case "02":
                    _Message = "Lỗi, tham số gửi từ merchant tới NgânLượng.vn chưa chính xác.";
                    break;
                case "03":
                    _Message = "Lỗi, mã merchant không tồn tại hoặc merchant đang bị khóa kết nối tới NgânLượng.vn";
                    break;
                case "04":
                    _Message = "Lỗi, mã checksum không chính xác";
                    break;
                case "05":
                    _Message = "Tài khoản nhận tiền nạp của merchant không tồn tại";
                    break;
                case "06":
                    _Message = "Tài khoản nhận tiền nạp của  merchant đang bị khóa hoặc bị phong tỏa, không thể thực hiện được giao dịch nạp tiền";
                    break;
                case "07":
                    _Message = "Thẻ đã được sử dụng";
                    break;
                case "08":
                    _Message = "Thẻ bị khóa";
                    break;
                case "09":
                    _Message = "Thẻ hết hạn sử dụng";
                    break;
                case "10":
                    _Message = "Thẻ chưa được kích hoạt hoặc không tồn tại";
                    break;
                case "11":
                    _Message = "Mã thẻ sai định dạng";
                    break;
                case "12":
                    _Message = "Sai số serial của thẻ";
                    break;
                case "13":
                    _Message = "Mã thẻ và số serial không khớp";
                    break;
                case "14":
                    _Message = "Thẻ không tồn tại";
                    break;
                case "15":
                    _Message = "Thẻ không sử dụng được";
                    break;
                case "16":
                    _Message = "Số lần tưử của thẻ vượt quá giới hạn cho phép";
                    break;
                case "17":
                    _Message = "Hệ thống Telco bị lỗi hoặc quá tải, thẻ chưa bị trừ";
                    break;
                case "18":
                    _Message = "Hệ thống Telco  bị lỗi hoặc quá tải, thẻ có thể bị trừ, cần phối hợp với nhà mạng để đối soát";
                    break;
                case "19":
                    _Message = "Kết nối NgânLượng với Telco bị lỗi, thẻ chưa bị trừ.";
                    break;
                case "20":
                    _Message = "Kết nối tới Telco thành công, thẻ bị trừ nhưng chưa cộng tiền trên NgânLượng.vn";
                    break;
                case "99":
                    _Message = "Lỗi tuy nhiên lỗi chưa được định nghĩa hoặc chưa xác định được nguyên nhân";
                    break;
            }
            return _Message;
        }
    }

    #region entity RequestCheckOrder
    public class RequestCheckOrder
    {
        private String _Funtion = "GetTransactionDetail";

        public String Funtion
        {
            get { return _Funtion; }
        }

        private String _Version = "3.1";

        public String Version
        {
            get { return _Version; }
        }
        private String _Merchant_id = String.Empty;
        public String Merchant_id
        {
            get { return _Merchant_id; }
            set { _Merchant_id = value; }
        }
        private String _Merchant_password = String.Empty;

        public String Merchant_password
        {
            get { return _Merchant_password; }
            set { _Merchant_password = value; }
        }
        private String _token = String.Empty;

        public String Token
        {
            get { return _token; }
            set { _token = value; }
        }
    }


    public class ResponseCheckOrder
    {
        private string error_code = string.Empty;

        public string errorCode
        {
            get { return error_code; }
            set { error_code = value; }
        }
        private string error_description = string.Empty;

        public string description
        {
            get { return error_description; }
            set { error_description = value; }
        }
        private string time_limit = string.Empty;

        public string timeLimit
        {
            get { return time_limit; }
            set { time_limit = value; }
        }
        private string _token = string.Empty;

        public string token
        {
            get { return _token; }
            set { _token = value; }
        }
        private string transaction_id = string.Empty;

        public string transactionId
        {
            get { return transaction_id; }
            set { transaction_id = value; }
        }
        private string amount = string.Empty;

        public string paymentAmount
        {
            get { return amount; }
            set { amount = value; }
        }
        private string _order_code = string.Empty;

        public string order_code
        {
            get { return _order_code; }
            set { _order_code = value; }
        }
        private string transaction_type = string.Empty;

        public string transactionType
        {
            get { return transaction_type; }
            set { transaction_type = value; }
        }
        private string transaction_status = string.Empty;

        public string transactionStatus
        {
            get { return transaction_status; }
            set { transaction_status = value; }
        }
        private string payer_name = string.Empty;

        public string payerName
        {
            get { return payer_name; }
            set { payer_name = value; }
        }
        private string payer_email = string.Empty;

        public string payerEmail
        {
            get { return payer_email; }
            set { payer_email = value; }
        }
        private string payer_mobile = string.Empty;

        public string payerMobile
        {
            get { return payer_mobile; }
            set { payer_mobile = value; }
        }
        private string receiver_name = string.Empty;

        public string merchantName
        {
            get { return receiver_name; }
            set { receiver_name = value; }
        }
        private string receiver_address = string.Empty;

        public string merchantAddress
        {
            get { return receiver_address; }
            set { receiver_address = value; }
        }
        private string receiver_mobile = string.Empty;

        public string merchantMobile
        {
            get { return receiver_mobile; }
            set { receiver_mobile = value; }
        }
        private string payment_method = string.Empty;

        public string paymentMethod
        {
            get { return payment_method; }
            set { payment_method = value; }
        }
    }

    #endregion

    #region Entity RequestInfo
    public class RequestInfo
    {
        private String _Funtion = "SetExpressCheckout";

        public String Funtion
        {
            get { return _Funtion; }
        }

        private String _Version = "3.1";

        public String Version
        {
            get { return _Version; }
        }

        private String _cur_code = String.Empty;
        public String cur_code
        {
            get { return _cur_code; }
            set { _cur_code = value; }
        }

        private String _discount_amount = String.Empty;
        public String Discount_amount
        {
            get { return _discount_amount; }
            set { _discount_amount = value; }
        }

        private String _Merchant_id = String.Empty;
        public String Merchant_id
        {
            get { return _Merchant_id; }
            set { _Merchant_id = value; }
        }
        private String _Receiver_email = String.Empty;

        public String Receiver_email
        {
            get { return _Receiver_email; }
            set { _Receiver_email = value; }
        }
        private String _Merchant_password = String.Empty;

        public String Merchant_password
        {
            get { return _Merchant_password; }
            set { _Merchant_password = value; }
        }
        private String _Order_code = String.Empty;

        public String Order_code
        {
            get { return _Order_code; }
            set { _Order_code = value; }
        }
        private String _Total_amount = String.Empty;

        public String Total_amount
        {
            get { return _Total_amount; }
            set { _Total_amount = value; }
        }
        private String _Payment_method = String.Empty;

        public String Payment_method
        {
            get { return _Payment_method; }
            set { _Payment_method = value; }
        }
        private String _Payment_type = String.Empty;

        public String Payment_type
        {
            get { return _Payment_type; }
            set { _Payment_type = value; }
        }
        private String _bank_code = String.Empty;

        public String bank_code
        {
            get { return _bank_code; }
            set { _bank_code = value; }
        }
        private String _order_description = String.Empty;

        public String order_description
        {
            get { return _order_description; }
            set { _order_description = value; }
        }
        private String _fee_shipping = String.Empty;

        public String fee_shipping
        {
            get { return _fee_shipping; }
            set { _fee_shipping = value; }
        }
        private String _tax_amount = String.Empty;

        public String tax_amount
        {
            get { return _tax_amount; }
            set { _tax_amount = value; }
        }

        private String _return_url = String.Empty;

        public String return_url
        {
            get { return _return_url; }
            set { _return_url = value; }
        }
        private String _cancel_url = String.Empty;

        public String cancel_url
        {
            get { return _cancel_url; }
            set { _cancel_url = value; }
        }
        private String _time_limit = String.Empty;

        public String time_limit
        {
            get { return _time_limit; }
            set { _time_limit = value; }
        }
        private String _Buyer_fullname = String.Empty;

        public String Buyer_fullname
        {
            get { return _Buyer_fullname; }
            set { _Buyer_fullname = value; }
        }
        private String _Buyer_email = String.Empty;

        public String Buyer_email
        {
            get { return _Buyer_email; }
            set { _Buyer_email = value; }
        }
        private String _Buyer_mobile = String.Empty;

        public String Buyer_mobile
        {
            get { return _Buyer_mobile; }
            set { _Buyer_mobile = value; }
        }
    }
    public class ResponseInfo
    {
        private String _error_code = string.Empty;

        public String Error_code
        {
            get { return _error_code; }
            set { _error_code = value; }
        }
        private String _checkout_url = String.Empty;

        public String Checkout_url
        {
            get { return _checkout_url; }
            set { _checkout_url = value; }
        }
        private String _Token = String.Empty;

        public String Token
        {
            get { return _Token; }
            set { _Token = value; }
        }

        private String _description = String.Empty;

        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
    #endregion
}