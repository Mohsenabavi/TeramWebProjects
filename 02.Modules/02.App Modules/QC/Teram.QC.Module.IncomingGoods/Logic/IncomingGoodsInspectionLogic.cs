using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;
using Teram.ServiceContracts;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.IncomingGoods.Logic
{

    public class IncomingGoodsInspectionLogic : BusinessOperations<IncomingGoodsInspectionModel, IncomingGoodsInspection, int>, IIncomingGoodsInspectionLogic
    {
        private readonly IUserPrincipal userPrincipal;
        private readonly IUserSharedService userSharedService;
        private readonly IServiceProvider serviceProvider;
        private readonly IIncomingGoodsInspectionCartableItemLogic incomingGoodsInspectionCartableItemLogic;

        public IncomingGoodsInspectionLogic(IPersistenceService<IncomingGoodsInspection> service, IUserPrincipal userPrincipal,
            IUserSharedService userSharedService, IServiceProvider serviceProvider,
            IIncomingGoodsInspectionCartableItemLogic incomingGoodsInspectionCartableItemLogic) : base(service)
        {
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.serviceProvider=serviceProvider??throw new ArgumentNullException(nameof(serviceProvider));
            this.incomingGoodsInspectionCartableItemLogic=incomingGoodsInspectionCartableItemLogic??throw new ArgumentNullException(nameof(incomingGoodsInspectionCartableItemLogic));
        }
        public BusinessOperationResult<List<IncomingGoodsInspectionModel>> GetByFilter(List<InspectionFormStatus> inspectionFormStatuses, bool isProductionManager, bool IsAdmin, string? qualityInspectionNumber, string? goodsTitle,string vendorName, InspectionFormStatus? inspectionFormStatus, int? start = null, int? length = null)
        {
            var query = CreateFilterExpression(inspectionFormStatuses, isProductionManager, IsAdmin, qualityInspectionNumber, goodsTitle, vendorName, inspectionFormStatus);
            if (start.HasValue && length.HasValue)
            {
                return GetData<IncomingGoodsInspectionModel>(query, row: start.Value, max: length.Value, orderByMember: "CreateDate", orderByDescending: true);
            }
            return GetData<IncomingGoodsInspectionModel>(query, null, null, false, null);
        }

        public BusinessOperationResult<IncomingGoodsInspectionModel> GetById(int id)
        {
            return GetFirst<IncomingGoodsInspectionModel>(x => x.IncomingGoodsInspectionId == id);
        }
        private Expression<Func<IncomingGoodsInspection, bool>> CreateFilterExpression(List<InspectionFormStatus> inspectionFormStatuses, bool isProductionManager, bool IsAdmin, string? qualityInspectionNumber, string? goodsTitle, string? vendorName, InspectionFormStatus? inspectionFormStatus)
        {
            Expression<Func<IncomingGoodsInspection, bool>> query = x => true;

            if (!IsAdmin)
            {
                if (isProductionManager && inspectionFormStatuses.Count!=0)
                {
                    query = query.AndAlso(x => inspectionFormStatuses.Contains(x.InspectionFormStatus));
                }
                else
                {
                    var cartableItems = incomingGoodsInspectionCartableItemLogic.GetByUserId(userPrincipal.CurrentUserId);
                    if (cartableItems.ResultStatus == OperationResultStatus.Successful && cartableItems.ResultEntity is not null)
                    {
                        var incomingGoodsInspectionIds = cartableItems.ResultEntity.Select(x => x.IncomingGoodsInspectionId).ToList();
                        query = query.AndAlso(x => incomingGoodsInspectionIds.Contains(x.IncomingGoodsInspectionId) && inspectionFormStatuses.Contains(x.InspectionFormStatus));
                    }
                }
            }

            if (inspectionFormStatus.HasValue)
            {
                query = query.AndAlso(x => x.InspectionFormStatus == inspectionFormStatus);
            }

            if (!string.IsNullOrEmpty(qualityInspectionNumber))
            {
                query = query.AndAlso(x => x.QualityInspectionNumber == qualityInspectionNumber);
            }
            if (!string.IsNullOrEmpty(goodsTitle))
            {
                query = query.AndAlso(x => x.GoodsTitle.Contains(goodsTitle));
            }

            if (!string.IsNullOrEmpty(vendorName))
            {
                query = query.AndAlso(x => x.VendorName.Contains(vendorName));
            }
            return query;
        }
        public CartableItemsReturnModel GetStepCartableItems(IncomingGoodsInspectionModel model, string comments)
        {

            var result = new CartableItemsReturnModel();

            switch (model.InspectionFormStatus)
            {

                case InspectionFormStatus.None:

                    if ((model.IsSampleGoods || model.HasFunctionalTest) && model.NeedToRefferal)
                    {
                        if (!userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:FromCreatorToProductionSupervisorCartablePermission"))
                        {
                            result.hasPermission=false;
                            return result;
                        }
                        return GetSuppervisorCartableItems(model, comments);
                    }
                    if ((model.IsSampleGoods || model.HasFunctionalTest) && !model.NeedToRefferal)
                    {
                        if (!userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:FromCreatorToProductionManagerCartablePermission"))
                        {
                            result.hasPermission=false;
                            return result;
                        }
                        return GetProductionManagerCartableItems(model, comments);
                    }

                    result.hasPermission=true;
                    return result;

                case InspectionFormStatus.ReferralToSupervisor:

                    if (!userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:FromSupervisorToProductionMangerCartablePermission"))
                    {
                        result.hasPermission=false;
                        return result;
                    }
                    return GetProductionManagerCartableItems(model, comments);

                case InspectionFormStatus.ReferralToProductionManager:
                    if (!model.IsSampleGoods)
                    {
                        if (!userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:FromProductionManagerToCreatorPermission"))
                        {
                            result.hasPermission=false;
                            return result;
                        }
                        return GetCreatorCartableItems(model, comments);
                    }
                    else
                    {
                        if (!userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:FromProductionManagerToQCManagerPermission"))
                        {
                            result.hasPermission=false;
                            return result;
                        }
                        return GetQCManagerCartableItems(model, comments);
                    }
                case InspectionFormStatus.ReferralToQCManager:

                    if (!userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:FromQCManagerToCreatorCartablePermission"))
                    {
                        result.hasPermission=false;
                        return result;
                    }
                    return GetCreatorCartableItems(model, comments);

                case InspectionFormStatus.ReferralToCreator:

                    return result;

                case InspectionFormStatus.ProcessCompleted:

                    return result;
                default:
                    return result;
            }
        }

        private CartableItemsReturnModel GetSuppervisorCartableItems(IncomingGoodsInspectionModel model, string comments)
        {

            var listOfCartableItems = new List<IncomingGoodsInspectionCartableItemModel>();

            var result = new CartableItemsReturnModel();


            if (model.ReferralUserId != null)
            {

                var cartableItems = new IncomingGoodsInspectionCartableItemModel
                {
                    Comments=(comments!=null) ? comments : " ",
                    IsApproved=false,
                    UserId=model.ReferralUserId.Value,
                    ReferredBy=userPrincipal.CurrentUserId,
                    InputDate=DateTime.Now,
                    IncomingGoodsInspectionId=model.IncomingGoodsInspectionId,
                };
                listOfCartableItems.Add(cartableItems);
            }

            else
            {
                var userInRole = userSharedService.GetUsersInRole("ProductionSupervisor").Result;
                var productionManagerUserIds = userInRole.Select(x => x.UserId).ToList();

                foreach (var productionManagerUserId in productionManagerUserIds)
                {
                    var cartableItems = new IncomingGoodsInspectionCartableItemModel
                    {
                        Comments=(comments!=null) ? comments : " ",
                        IsApproved=false,
                        UserId=productionManagerUserId,
                        ReferredBy=userPrincipal.CurrentUserId,
                        InputDate=DateTime.Now,
                        IncomingGoodsInspectionId=model.IncomingGoodsInspectionId,
                    };
                    listOfCartableItems.Add(cartableItems);
                }
            }

            result.IncomingGoodsInspectionCartableItems = listOfCartableItems;
            result.InspectionFormStatus=InspectionFormStatus.ReferralToSupervisor;
            result.hasPermission=true;
            return result;
        }

        private CartableItemsReturnModel GetProductionManagerCartableItems(IncomingGoodsInspectionModel model, string comments)
        {
            var result = new CartableItemsReturnModel();

            var userInRole = userSharedService.GetUsersInRole("ProductionManager").Result;
            var productionManagerUserIds = userInRole.Select(x => x.UserId).ToList();

            var listOfCartableItems = new List<IncomingGoodsInspectionCartableItemModel>();

            foreach (var productionManagerUserId in productionManagerUserIds)
            {
                var cartableItems = new IncomingGoodsInspectionCartableItemModel
                {
                    Comments=(comments!=null) ? comments : " ",
                    IsApproved=false,
                    UserId=productionManagerUserId,
                    ReferredBy=userPrincipal.CurrentUserId,
                    InputDate=DateTime.Now,
                    IncomingGoodsInspectionId=model.IncomingGoodsInspectionId,
                };
                listOfCartableItems.Add(cartableItems);
            }
            result.IncomingGoodsInspectionCartableItems= listOfCartableItems;
            result.InspectionFormStatus=InspectionFormStatus.ReferralToProductionManager;
            result.hasPermission=true;
            return result;
        }

        private CartableItemsReturnModel GetCreatorCartableItems(IncomingGoodsInspectionModel model, string comments)
        {

            var result = new CartableItemsReturnModel();

            var itemsList = new List<IncomingGoodsInspectionCartableItemModel>();

            var creatorId = model.CreatedBy;
            var cartableItem = new IncomingGoodsInspectionCartableItemModel
            {
                Comments=(comments!=null) ? comments : " ",
                IsApproved=false,
                UserId=creatorId,
                ReferredBy=userPrincipal.CurrentUserId,
                InputDate=DateTime.Now,
                IncomingGoodsInspectionId=model.IncomingGoodsInspectionId,
            };
            itemsList.Add(cartableItem);
            result.IncomingGoodsInspectionCartableItems = itemsList;

            result.InspectionFormStatus=InspectionFormStatus.ReferralToCreator;
            result.hasPermission=true;

            return result;
        }

        private CartableItemsReturnModel GetQCManagerCartableItems(IncomingGoodsInspectionModel model, string comments)
        {
            var result = new CartableItemsReturnModel();

            var userInRole = userSharedService.GetUsersInRole("QCManager").Result;
            var qCManagerUserIds = userInRole.Select(x => x.UserId).ToList();

            var listOfCartableItems = new List<IncomingGoodsInspectionCartableItemModel>();

            foreach (var productionManagerUserId in qCManagerUserIds)
            {
                var cartableItems = new IncomingGoodsInspectionCartableItemModel
                {
                    Comments=(comments!=null) ? comments : " ",
                    IsApproved=false,
                    UserId=productionManagerUserId,
                    ReferredBy=userPrincipal.CurrentUserId,
                    InputDate=DateTime.Now,
                    IncomingGoodsInspectionId=model.IncomingGoodsInspectionId,
                };
                listOfCartableItems.Add(cartableItems);
            }
            result.IncomingGoodsInspectionCartableItems=listOfCartableItems;
            result.InspectionFormStatus = InspectionFormStatus.ReferralToQCManager;
            result.hasPermission=true;
            return result;
        }
    }

}
