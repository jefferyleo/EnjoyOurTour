using EnjoyOurTour.Helpers.Services;
using EnjoyOurTour.Models;
using EnjoyOurTour.Models.ViewModel.Admin;
using EnjoyOurTour.Models.ViewModel.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnjoyOurTour.Controllers
{
    public class AgentController : Controller
    {
        // GET: Agent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgentTransaction()
        {
            using (var db = new TourEntities())
            {
                int userId = MetadataServices.GetCurrentUser().UserId;
                List<AgentTransactionViewModel> agentTransaction = (from data in db.AgentTransaction
                                                                              where data.AgentTransactionId == userId
                                                                              select new AgentTransactionViewModel()
                                                                              {
                                                                                  AgentTransactionId = data.AgentTransactionId,
                                                                                  AgentId = data.AgentId,
                                                                                  CreditAmount = data.CreditAmount,
                                                                                  FinalAmount = data.FinalAmount,
                                                                                  CreditBonus = data.CreditBonus,
                                                                                  FinalBonus = data.FinalBonus,
                                                                                  DebitAmount = data.DebitAmount,
                                                                                  DebitBonus = data.DebitBonus,
                                                                                  CreatedAt = data.CreatedAt,
                                                                                  CreatedBy = data.CreatedBy
                                                                              }).ToList();
                return View(agentTransaction);
            }
        }

        public ActionResult UpdateAgentProfile()
        {
            using (var db = new TourEntities())
            {
                int userId = MetadataServices.GetCurrentUser().UserId;

                var agent = db.Agent.Where(x => x.UserId == userId).FirstOrDefault();
                if (agent == null)
                {
                    return new HttpNotFoundResult("Agent not found");
                }
                var user = db.User.Where(e => e.UserId == agent.UserId).FirstOrDefault();

                return View(new EditAgentProfileViewModel()
                {
                    AgentId = agent.AgentId,
                    EmailAddress = user.EmailAddress,
                    DOB = agent.DOB,
                    DOBString = agent.DOB.ToString("yyyy-MM-dd HH:mm"),
                    AgentName = agent.AgentName,
                    BankName = agent.BankName,
                    BankAccountNumber = agent.BankAccountNumber,
                    Address = agent.Address,
                    NRIC = agent.NRIC,
                    PhoneNumber = agent.PhoneNumber,
                    UserId = user.UserId
                });
            }
        }

        [HttpPost]
        public ActionResult UpdateAgentProfile(EditAgentProfileViewModel model)
        {
            using (var db = new TourEntities())
            {
                if (ModelState.IsValid)
                {
                    var agent = db.Agent.Find(model.AgentId);
                    var user = db.User.Find(MetadataServices.GetCurrentUser().UserId);

                    if (agent == null || user == null)
                    {
                        return new HttpNotFoundResult("Agent not found.");
                    }

                    bool chkIC = db.Agent.Where(x => x.NRIC == model.NRIC).Any();

                    if (!chkIC)
                    {
                        agent.AgentCode = Convert.ToInt32(model.NRIC.Substring(model.NRIC.Length - 6));
                    }

                    user.EmailAddress = model.EmailAddress;
                    agent.AgentName = model.AgentName;
                    agent.BankName = model.BankName;
                    agent.BankAccountNumber = model.BankAccountNumber;
                    agent.Address = model.Address;
                    agent.DOB = model.DOB;
                    agent.NRIC = model.NRIC;
                    agent.PhoneNumber = model.PhoneNumber;
                    agent.UserId = user.UserId;
                    agent.UpdatedAt = MetadataServices.GetCurrentDate();
                    agent.UpdatedBy = MetadataServices.GetCurrentUser().Username;

                    db.SaveChanges();
                    return RedirectToAction("UpdateAgentProfile", "Agent");
                }
                return View(model);
            }
        }
    }
}