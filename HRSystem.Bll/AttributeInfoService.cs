﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Data;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Bll
{
    public class AttributeInfoService : IAttributeInfoService
    {
        private readonly HrSystemDb _db;

        public AttributeInfoService(HrSystemDb db)
        {
            ArgumentHelper.EnsureNotNull(nameof(db), db);
            
            _db = db;
        }

        public async Task<IEnumerable<AttributeInfo>> GetAll()
        {
            return await _db.AttributeInfos.ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsExists(int attributeInfoId)
        {
            return await _db.AttributeInfos
                .AnyAsync(a => a.Id == attributeInfoId)
                .ConfigureAwait(false);
        }

        public async Task<bool> IsExists(string name)
        {
            return await _db.AttributeInfos
                .AnyAsync(a => a.Name == name)
                .ConfigureAwait(false);
        }

        public async Task Create(AttributeInfo attribute)
        {
            await _db.AttributeInfos.AddAsync(attribute).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(AttributeInfo attribute)
        {
            var item = await _db.AttributeInfos.SingleAsync(a => a.Id == attribute.Id).ConfigureAwait(false);
            item.Update(attribute);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<AttributeInfo> GetById(int id)
        {
            return await _db.AttributeInfos
                .SingleAsync(a => a.Id == id)
                .ConfigureAwait(false);
        }

        public Task Delete(AttributeInfo attributeInfo)
        {
            var attributeBases = _db.AttributeBases.Where(a => a.AttributeInfoId == attributeInfo.Id);
            _db.AttributeBases.RemoveRange(attributeBases);
            _db.AttributeInfos.Remove(attributeInfo);
            
            return _db.SaveChangesAsync();
        }
    }
}