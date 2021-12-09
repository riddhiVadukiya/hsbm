[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HSBM.Web.MVCGridConfig), "RegisterGrids")]

namespace HSBM.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using HSBM.Service.Contracts;
    using HSBM.EntityModel.CountryMaster;

    public static class MVCGridConfig
    {
        public static void RegisterGrids()
        {

            MVCGridDefinitionTable.Add("CountryGridview", new MVCGridBuilder<CountryMaster>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
                .WithPaging(paging: true, itemsPerPage: 10, allowChangePageSize: true, maxItemsPerPage: 100)
                .WithAdditionalQueryOptionNames("search")
                .AddColumns(cols =>
                {
                    //cols.Add("Id")
                    //    .WithValueExpression((p, c) => c.UrlHelper.Action("UpdateCountry", "Locations", new { id = p.Id }))
                    //    .WithValueTemplate("<a href='{Value}'>{Model.Id}</a>", false)
                    //    .WithPlainTextValueExpression(p => p.Id.ToString());
                    cols.Add("CountryName").WithHeaderText("CountryName")
                        .WithVisibility(true, true)
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(p => p.CountryName);
                    cols.Add("Code").WithHeaderText("Code")
                        .WithVisibility(true, true)
                        .WithValueExpression(p => p.Code);
                    cols.Add("IsActive")
                        .WithSortColumnData("IsActive")
                        .WithVisibility(visible: true, allowChangeVisibility: true)
                        .WithHeaderText("Status")
                        .WithValueExpression(p => p.IsActive ? "Active" : "Inactive")
                        .WithCellCssClassExpression(p => p.IsActive ? "success" : "danger");
                    cols.Add("ViewLink").WithSorting(false)
                        .WithHeaderText("Action")
                        .WithHtmlEncoding(false)
                        //.WithValueExpression((p, c) => c.UrlHelper.Action("UpdateCountry", "Locations", new { id = p.Id }))
                        //.WithValueTemplate("<a href='{Value}'>Edit</a>|<a href='{Value}'>DeleteCountry</a>")
                        //.WithValueExpression((p, c) => c.UrlHelper.Action("DeleteCountry", "Locations", new { id = p.Id }))
                        //.WithValueTemplate("<a href='{Value}'>Delete</a>")
                        .WithValueTemplate("<a href='/Locations/UpdateCountry/{Model.Id}' title='View'>Edit</a> | <a href='/Locations/DeleteCountry/{Model.Id}' title='Edit'>Delete</a>");
                })
                //.WithAdditionalSetting(MVCGrid.Rendering.BootstrapRenderingEngine.SettingNameTableClass, "notreal") // Example of changing table css class
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    int totalRecords;
                    var repo = DependencyResolver.Current.GetService<ILocationsService>();
                    string globalSearch = options.GetAdditionalQueryOptionString("search");
                    string sortColumn = options.GetSortColumnData<string>();
                    //var items = repo.GetAllCountryForGrid(out totalRecords, globalSearch, options.GetLimitOffset(), options.GetLimitRowcount(), sortColumn, options.SortDirection == SortDirection.Dsc);

                    List<CountryMaster> lst = new List<CountryMaster>();


                    if (options.GetLimitOffset().HasValue || options.GetLimitRowcount().HasValue)
                    {
                        lst = repo.GetAllCountryForGrid(out totalRecords, globalSearch, options.GetLimitOffset().Value, options.GetLimitRowcount().Value, sortColumn, (options.SortDirection == SortDirection.Dsc ? "Desc" : "Asc"));
                    }
                    else
                    {
                        lst = repo.GetAllCountryForGrid(out totalRecords, globalSearch, 0, 0, sortColumn, (options.SortDirection == SortDirection.Dsc ? "Desc" : "Asc"));
                    }


                    return new QueryResult<CountryMaster>()
                    {
                        Items = lst,
                        TotalRecords = totalRecords
                    };
                }));


            /*
            MVCGridDefinitionTable.Add("UsageExample", new MVCGridBuilder<YourModelItem>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("UniqueColumnName")
                        .WithHeaderText("Any Header")
                        .WithValueExpression(i => i.YourProperty); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("UrlExample")
                        .WithHeaderText("Edit")
                        .WithValueExpression((i, c) => c.UrlHelper.Action("detail", "demo", new { id = i.Id }));
                })
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>

                    return new QueryResult<YourModelItem>()
                    {
                        Items = new List<YourModelItem>(),
                        TotalRecords = 0 // if paging is enabled, return the total number of records of all pages
                    };

                })
            );
            */
        }
    }

    public class MVCGridToolbarModel
    {
        public MVCGridToolbarModel()
        {

        }

        public MVCGridToolbarModel(string gridName)
        {
            MVCGridName = gridName;
        }

        public string MVCGridName { get; set; }
        public bool PageSize { get; set; }
        public bool ColumnVisibility { get; set; }
        public bool Export { get; set; }
        public bool GlobalSearch { get; set; }
    }
}