using EnjoyOurTour.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace EnjoyOurTour.Helpers
{
    public enum TransactionActivity
    {
        SignUp = 1,
        TopUp = 2,
        RedeemPackage = 3,
        RedeemPoint = 4
    }

    public enum TransactionStatus
    {
        Pending = 1,
        Approve = 2,
        ApproveWithoutPay = 3,
        Reject = 4    
    }
}