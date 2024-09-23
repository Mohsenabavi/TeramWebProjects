using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.TicketRegister.Entities;
using Teram.HR.Module.TicketRegister.Logic.Interfaces;
using Teram.HR.Module.TicketRegister.Models;

namespace Teram.HR.Module.TicketRegister.Logic
{

    public class SeatLogic : BusinessOperations<SeatModel, Seat, int>, ISeatLogic
    {
        private readonly IAreaLogic areaLogic;
        private readonly IAreaRowLogic areaRowLogic;

        public SeatLogic(IPersistenceService<Seat> service, IAreaLogic areaLogic, IAreaRowLogic areaRowLogic) : base(service)
        {
            this.areaLogic=areaLogic??throw new ArgumentNullException(nameof(areaLogic));
            this.areaRowLogic=areaRowLogic??throw new ArgumentNullException(nameof(areaRowLogic));
        }

        public BusinessOperationResult<List<SeatModel>> CreateSeats(int AreaId)
        {
            var result = new BusinessOperationResult<List<SeatModel>>();

            var area = areaLogic.GetByAreaId(AreaId);

            if (area.ResultStatus != OperationResultStatus.Successful || area.ResultEntity is null)
            {
                result.SetErrorMessage("Area Not Found");
                return result;
            }
            var insertListModel = new List<SeatModel>();

            if (area.ResultEntity.AreaRows!=null)
            {
                foreach (var areaRow in area.ResultEntity.AreaRows)
                {
                    for (int i = 1; i < areaRow.SeatCount+1; i++)
                    {

                        insertListModel.Add(new SeatModel
                        {
                            SeatNumber = i,
                            AreaRowId = areaRow.AreaRowId,
                            RowVersion=Guid.NewGuid().ToByteArray(),
                        });
                    }
                }
                var insertResult =  BulkInsertAsync(insertListModel).Result;
                result.SetSuccessResult(insertResult.ResultEntity);
                return result;
            }
            return result;
        }
        public BusinessOperationResult<List<SeatModel>> GetAreaSeats(int id)
        {
            return GetData<SeatModel>(x => x.AreaRow.AreaId==id);
        }
    }
}
