﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;
using static OutlookInspired.Module.Services.Internal.ReportsExtensions;

namespace OutlookInspired.Module.Features.Customers{
    public class ReportController:ViewController{
        public const string ReportActionId = "CustomerReport";
        public ReportController(){
            TargetObjectType = typeof(Customer);
            ReportAction = new SingleChoiceAction(this, ReportActionId, PredefinedCategory.Reports){
                ImageName = "BO_Report", SelectionDependencyType = SelectionDependencyType.RequireSingleObject,PaintStyle = ActionItemPaintStyle.Image,
                Items ={
                    new ChoiceActionItem("Sales",SalesSummaryReport){ImageName ="CustomerQuickSales"},
                    new ChoiceActionItem("Employees",Contacts){ImageName = "EmployeeProfile"},
                    new ChoiceActionItem("Locations",LocationsReport){ImageName = "CustomerQuickLocations"}
                },
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            ReportAction.Executed+=ReportActionOnExecuted;
            ReportAction.Enabled.ResultValueChanged += (sender, e) => {
                if (!e.NewValue){

                }
            };
        }

        public SingleChoiceAction ReportAction{ get; }

        private void ReportActionOnExecuted(object sender, ActionBaseEventArgs e){
            var selectedItemData = (string)ReportAction.SelectedItem.Data;
            if (selectedItemData == SalesSummaryReport){
                ReportAction.ShowReportPreview(View.ObjectTypeInfo.Type, CriteriaOperator.FromLambda<OrderItem>(item 
                    => item.Order.Customer.ID == ((Customer)View.CurrentObject).ID),"Customer");
            }
            else if (selectedItemData == LocationsReport){
                ReportAction.ShowReportPreview(View.ObjectTypeInfo.Type,CriteriaOperator.FromLambda<Customer>(customer
                    => customer.ID == ((Customer)View.CurrentObject).ID));
            }
            else if (selectedItemData == Contacts){
                ReportAction.ShowReportPreview(View.ObjectTypeInfo.Type,CriteriaOperator.FromLambda<CustomerEmployee>(customerEmployee
                    => customerEmployee.Customer.ID == ((Customer)View.CurrentObject).ID));
            }
        }

        protected override void OnViewControllersActivated(){
            base.OnViewControllersActivated();
            if (!(Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active))return;
            ReportAction.ApplyReportProtection();
        }

    }
}