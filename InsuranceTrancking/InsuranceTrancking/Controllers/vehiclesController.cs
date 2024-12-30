using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InsuranceTrancking.Models;

namespace InsuranceTrancking.Controllers
{
    public class vehiclesController : MController
    {
        private Model1 db = new Model1();

        // GET: vehicles
        public ActionResult Index()
        {
            var vehicles = db.vehicles.Include(v => v.customers);
            return View(vehicles.ToList());
        }

        // GET: vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehicles vehicles = db.vehicles.Find(id);
            if (vehicles == null)
            {
                return HttpNotFound();
            }
            return View(vehicles);
        }

        // GET: vehicles/Create
        public ActionResult Create()
        {


            ViewBag.Customers = db.customers.ToList() ?? new List<InsuranceTrancking.Models.customers>();
            return View();

        }

        
        [HttpPost]
        
       



        // GET: vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehicles vehicles = db.vehicles.Find(id);
            if (vehicles == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customers = db.customers.ToList();
            return View(vehicles);
        }

        // POST: vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,VehicleID,Brand,Model,Plate,Year,CustomerClaimDate")] vehicles vehicles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicles).State = EntityState.Modified;
                db.SaveChanges();//->hata
                return RedirectToAction("Index");
            }
            ViewBag.Customers = db.customers.ToList();
            return View(vehicles);
        }

        // GET: vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehicles vehicles = db.vehicles.Find(id);
            if (vehicles == null)
            {
                return HttpNotFound();
            }
            return View(vehicles);
        }

        // POST: vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Find the vehicle
                    vehicles vehicle = db.vehicles.Find(id);
                    if (vehicle == null)
                    {
                        return HttpNotFound();
                    }

                    // Delete related accident reports linked to insurance policies
                    var insurancePolicies = db.insurance_policies.Where(i => i.VehicleID == id).ToList();
                    foreach (var policy in insurancePolicies)
                    {
                        var relatedAccidentReports = db.accident_reports.Where(ar => ar.PolicyID == policy.PolicyID).ToList();
                        if (relatedAccidentReports.Any())
                        {
                            db.accident_reports.RemoveRange(relatedAccidentReports);
                        }
                    }

                    // Delete related insurance policies
                    if (insurancePolicies.Any())
                    {
                        db.insurance_policies.RemoveRange(insurancePolicies);
                    }

                    // Delete related accident reports directly linked to the vehicle
                    var vehicleAccidentReports = db.accident_reports.Where(a => a.VehicleID == id).ToList();
                    if (vehicleAccidentReports.Any())
                    {
                        db.accident_reports.RemoveRange(vehicleAccidentReports);
                    }

                    // Delete the vehicle
                    db.vehicles.Remove(vehicle);

                    // Save changes and commit transaction
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback transaction in case of error
                    transaction.Rollback();
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"Error deleting vehicle with ID {id}: {ex.Message}");
                    throw;
                }
            }

            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
