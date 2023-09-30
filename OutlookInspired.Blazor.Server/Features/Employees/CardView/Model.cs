﻿using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Employees.CardView{
    public class Model:RootListViewComponentModel<Employee,Model>{
        protected override RenderFragment FragmentSelector(Model model) => CardView.Create(model);
    }
}