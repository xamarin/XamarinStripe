/*
 * Copyright 2011 Xamarin, Inc., Joe Dluzen
 *
 * Author(s):
 *  Gonzalo Paniagua Javier (gonzalo@xamarin.com)
 *  Joe Dluzen (jdluzen@gmail.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using Xamarin.Payments.Stripe;

namespace PaymentTest {
    class Program {
        static void Main (string [] args)
        {
            StripePayment payment = new StripePayment ("vtUQeOtUnYr7PGCLQ96Ul4zqpDUO4sOE");
            //TestSimpleCharge (payment);
            //TestCustomer (payment);
            //TestCustomerAndCharge (payment);
            //TestGetCharges (payment);
            //TestGetCustomers (payment);
            //TestCreateGetToken (payment);
            //TestCreatePlanGetPlan (payment);
            TestCreateSubscription (payment);
            TestCreateInvoiceItems (payment);
            //TestInvoices (payment);
            TestInvoices2 (payment);
        }

        static StripeCreditCardInfo GetCC ()
        {
            StripeCreditCardInfo cc = new StripeCreditCardInfo ();
            cc.CVC = "1234";
            cc.ExpirationMonth = 6;
            cc.ExpirationYear = 2012;
            cc.Number = "4242424242424242";
            return cc;
        }

        static StripePlanInfo GetPlanInfo ()
        {
            return new StripePlanInfo {
                Amount = 1999,
                ID = "myplan",
                Interval = StripeInterval.Month,
                Name = "My standard plan",
                TrialPeriod = 7
            };
        }

        static StripeInvoiceItemInfo GetInvoiceItemInfo ()
        {
            return new StripeInvoiceItemInfo {
                Amount = 1999,
                Description = "Invoice item: " + Guid.NewGuid ().ToString ()
            };
        }

        static void TestSimpleCharge (StripePayment payment)
        {
            StripeCreditCardInfo cc = GetCC ();
            StripeCharge charge = payment.Charge (5001, "usd", cc, "Test charge");
            Console.WriteLine (charge);
            string charge_id = charge.ID;
            StripeCharge charge_info = payment.GetCharge (charge_id);
            Console.WriteLine (charge_info);

            StripeCharge refund = payment.Refund (charge_info.ID);
            Console.WriteLine (refund.Created);
        }

        static void TestCustomer (StripePayment payment)
        {
            StripeCustomerInfo customer = new StripeCustomerInfo ();
            //customer.Card = GetCC ();
            StripeCustomer customer_resp = payment.CreateCustomer (customer);
            string customer_id = customer_resp.ID;
            StripeCustomer customer_info = payment.GetCustomer (customer_id);
            Console.WriteLine (customer_info);
            StripeCustomer ci2 = payment.DeleteCustomer (customer_id);
            if (ci2.Deleted == false)
                throw new Exception ("Failed to delete " + customer_id);
        }

        static void TestCustomerAndCharge (StripePayment payment)
        {
            StripeCustomerInfo customer = new StripeCustomerInfo ();
            //customer.Card = GetCC ();
            StripeCustomer response = payment.CreateCustomer (customer);
            string customer_id = response.ID;
            StripeCustomer customer_info = payment.GetCustomer (customer_id);
            Console.WriteLine (customer_info);
            StripeCustomerInfo info_update = new StripeCustomerInfo ();
            info_update.Card = GetCC ();
            StripeCustomer update_resp = payment.UpdateCustomer (customer_id, info_update);
            Console.Write ("Customer updated with CC. Press ENTER to continue...");
            Console.Out.Flush ();
            Console.ReadLine ();
            StripeCustomer ci2 = payment.DeleteCustomer (customer_id);
            if (ci2.Deleted == false)
                throw new Exception ("Failed to delete " + customer_id);
        }

        static void TestGetCharges (StripePayment payment)
        {
            List<StripeCharge> charges = payment.GetCharges (0, 10);
            Console.WriteLine (charges.Count);
        }

        static void TestGetCustomers (StripePayment payment)
        {
            List<StripeCustomer> customers = payment.GetCustomers (0, 10);
            Console.WriteLine (customers.Count);
        }

        static void TestCreateGetToken (StripePayment payment)
        {
            StripeCreditCardToken tok = payment.CreateToken (GetCC ());
            StripeCreditCardToken tok2 = payment.GetToken (tok.ID);
        }

        static void TestCreatePlanGetPlan (StripePayment payment)
        {
            StripePlan plan = CreatePlan (payment);
            int total;
            List<StripePlan> plans = payment.GetPlans (10, 10, out total);
            Console.WriteLine (total);
        }

        static StripePlan CreatePlan (StripePayment payment)
        {
            StripePlan plan = payment.CreatePlan (GetPlanInfo ());
            StripePlan plan2 = payment.GetPlan (plan.ID);
            //DeletePlan (plan2, payment);
            return plan2;
        }

        static StripePlan DeletePlan (StripePlan plan, StripePayment payment)
        {
            StripePlan deleted = payment.DeletePlan (plan.ID);
            return deleted;
        }

        static void TestCreateSubscription (StripePayment payment)
        {
            StripeCustomer cust = payment.CreateCustomer (new StripeCustomerInfo {
                Card = GetCC ()
            });
            //StripePlan temp = new StripePlan { ID = "myplan" };
            //DeletePlan (temp, payment);
            StripePlan plan = CreatePlan (payment);
            StripeSubscription sub = payment.Subscribe (cust.ID, new StripeSubscriptionInfo {
                Card = GetCC (),
                Plan = "myplan",
                Prorate = true
            });
            StripeSubscription sub2 = payment.GetSubscription (sub.CustomerID);
            TestDeleteSubscription (cust, payment);
            DeletePlan (plan, payment);
        }

        static StripeSubscription TestDeleteSubscription (StripeCustomer customer, StripePayment payment)
        {
            StripeSubscription sub = payment.Unsubscribe (customer.ID, true);
            return sub;
        }

        static void TestCreateInvoiceItems (StripePayment payment)
        {
            StripeCustomer cust = payment.CreateCustomer (new StripeCustomerInfo ());
            StripeInvoiceItemInfo info = GetInvoiceItemInfo ();
            info.CustomerID = cust.ID;
            StripeInvoiceItem item = payment.CreateInvoiceItem (info);
            StripeInvoiceItemInfo newInfo = GetInvoiceItemInfo ();
            newInfo.Description = "Invoice item: " + Guid.NewGuid ().ToString ();
            StripeInvoiceItem item2 = payment.UpdateInvoiceItem (item.ID, newInfo);
            StripeInvoiceItem item3 = payment.GetInvoiceItem (item2.ID);
            if (item.Description == item3.Description)
                throw new Exception ("Update failed");
            StripeInvoiceItem deleted = payment.DeleteInvoiceItem (item2.ID);
            if (!deleted.Deleted.HasValue && deleted.Deleted.Value)
                throw new Exception ("Delete failed");
            int total;
            List<StripeInvoiceItem> items = payment.GetInvoiceItems (10, 10, null, out total);
            Console.WriteLine (total);
            payment.DeleteCustomer (cust.ID);
        }

        static void TestInvoices (StripePayment payment)
        {
            List<StripeInvoice> invoices = payment.GetInvoices (10, 10);
            StripeInvoice inv = payment.GetInvoice (invoices [0].ID);
            StripeCustomer cust = payment.CreateCustomer (new StripeCustomerInfo ());
            StripeSubscription sub = payment.Subscribe (cust.ID, new StripeSubscriptionInfo {
                Card = GetCC ()
            });
            StripeInvoice inv2 = payment.GetUpcomingInvoice (cust.ID);
            payment.Unsubscribe (cust.ID, true);
            payment.DeleteCustomer (cust.ID);
        }

        static void TestInvoices2 (StripePayment payment)
        {
            StripeCustomer cust = payment.GetCustomer ("cus_ulcOcy5Seu2dpq");
            StripePlanInfo planInfo = new StripePlanInfo{
                Amount = 1999,
                ID = "testplan",
                Interval = StripeInterval.Month,
                Name = "The Test Plan",
            //TrialPeriod = 7
            };
            //payment.DeletePlan (planInfo.ID);
            StripePlan plan = payment.CreatePlan (planInfo);
            StripeSubscriptionInfo subInfo = new StripeSubscriptionInfo{
                Card = GetCC (),
                Plan = planInfo.ID,
                Prorate = true
            };
            StripeSubscription sub = payment.Subscribe (cust.ID, subInfo);
            payment.CreateInvoiceItem (new StripeInvoiceItemInfo {
                CustomerID = cust.ID,
                Amount = 1337,
                Description = "Test single charge"
            });

            int total;
            List<StripeInvoice> invoices = payment.GetInvoices (0, 10, cust.ID, out total);
            StripeInvoice upcoming = payment.GetUpcomingInvoice (cust.ID);
            payment.Unsubscribe (cust.ID, true);
            payment.DeletePlan (planInfo.ID);
            foreach (StripeInvoiceLineItem line in upcoming) {
                Console.WriteLine ("{0} for type {1}", line.Amount, line.GetType ());
            }

        }
    }
}
