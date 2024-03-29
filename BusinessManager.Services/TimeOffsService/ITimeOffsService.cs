﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessManager.Services.DTOs.Teams;
using BusinessManager.Services.DTOs.TimeOffs;

namespace BusinessManager.Services.TimeOffsService
{
    public interface ITimeOffsService
    {
        Task<ICollection<TimeOffRequestDto>> GetAllAsync(string userId);
        Task<TimeOffRequestDto?> GetByIdAsync(int requestId, bool disableTracking = true);
        Task<int> CreateAsync(string userId, CreateTimeOffRequestDto timeOffRequestDto);
        Task EditAsync(TimeOffRequestDto timeOffRequestDto);
        Task DeleteAsync(int requestId);
        Task<PaginatedTimeOffsCollectionDto?> GetPaginatedRequests(string userId, int? page, int? pageSize, string keyWord);
        IEnumerable<KeyValuePair<string, string>> GetAllRequestTypesAsKeyValuePairs();
        Task ApproveRequest(int id);
        Task DeclineRequest(int id);
    }
}
