using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web.Mvc;
using HansKindberg.Web.Paging.TestApplication.Models;
using HansKindberg.Web.Paging.TestApplication.Models.ViewModels;

namespace HansKindberg.Web.Paging.TestApplication.Controllers
{
	public class PagingController : Controller
	{
		#region Fields

		private const string _pagingSettingsSessionKey = "PagingSettings";

		#endregion

		#region Methods

		protected virtual PagingSettingsViewModel CreateDefaultPagingSettingsViewModel()
		{
			PagingResolver pagingResolver = new PagingResolver();

			return new PagingSettingsViewModel
				{
					MaximumNumberOfDisplayedPages = pagingResolver.MaximumNumberOfDisplayedPages,
					NumberOfRecords = 1000,
					PageIndexQueryStringKey = pagingResolver.PageIndexQueryStringKey,
					PageSize = pagingResolver.PageSize,
					PagingEnabled = pagingResolver.PagingEnabled
				};
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		protected virtual PagingSettingsViewModel GetOrCreatePagingSettingsViewModel()
		{
			// ReSharper disable PossibleNullReferenceException
			PagingSettingsViewModel model = this.HttpContext.Session[_pagingSettingsSessionKey] as PagingSettingsViewModel;
			// ReSharper restore PossibleNullReferenceException

			if(model == null)
			{
				model = this.CreateDefaultPagingSettingsViewModel();

				// ReSharper disable PossibleNullReferenceException
				this.HttpContext.Session[_pagingSettingsSessionKey] = model;
				// ReSharper restore PossibleNullReferenceException
			}

			return model;
		}

		public virtual ActionResult Index()
		{
			PagingSettingsViewModel pagingSettingsViewModel = this.GetOrCreatePagingSettingsViewModel();

			List<ListItem> records = new List<ListItem>();

			for(int i = 0; i < pagingSettingsViewModel.NumberOfRecords; i++)
			{
				ListItem listItem = new ListItem
					{
						Heading = string.Format(CultureInfo.InvariantCulture, "Heading {0}", i),
						Information = string.Format(CultureInfo.InvariantCulture, "Information {0}, information {0}, information {0}, information {0}, information {0}, information {0}, information {0}, information {0}, information {0}.", i)
					};

				records.Add(listItem);
			}

			PagingResolver pagingResolver = new PagingResolver
				{
					MaximumNumberOfDisplayedPages = pagingSettingsViewModel.MaximumNumberOfDisplayedPages,
					PageIndexQueryStringKey = pagingSettingsViewModel.PageIndexQueryStringKey,
					PageSize = pagingSettingsViewModel.PageSize,
					PagingEnabled = pagingSettingsViewModel.PagingEnabled
				};

			PagingViewModel model = new PagingViewModel();

			this.PopulateModel(model);

			model.Title = "Paging sample";

			model.PagingResult = pagingResolver.Resolve(records, this.HttpContext.Request.Url);

			model.Records = records;

			return this.View(model);
		}

		public virtual ActionResult Settings()
		{
			PagingSettingsViewModel model = this.GetOrCreatePagingSettingsViewModel();

			this.PopulateModel(model);

			model.Title = "Paging settings";

			return this.View(model);
		}

		[AllowAnonymous]
		[HttpPost]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual ActionResult Settings(PagingSettingsViewModel model)
		{
			if(model == null)
				throw new ArgumentNullException("model");

			if(this.ModelState.IsValid)
			{
				try
				{
					// ReSharper disable PossibleNullReferenceException
					this.HttpContext.Session[_pagingSettingsSessionKey] = model;
					// ReSharper restore PossibleNullReferenceException
				}
				catch(Exception exception)
				{
					this.ModelState.AddModelError("Save", exception);
				}
			}

			this.PopulateModel(model);

			model.Title = "Paging settings";

			return this.View(model);
		}

		#endregion
	}
}