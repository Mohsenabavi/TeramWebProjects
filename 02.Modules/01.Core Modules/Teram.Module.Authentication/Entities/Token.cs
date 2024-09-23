using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.Module.Authentication.Models;
using System.ComponentModel.DataAnnotations;

namespace Teram.Module.Authentication.Entities
{
    [Table("Tokens", Schema = "api")]
    public class Token : EntityBase
    {
        [Key]
        public Guid TokenId { get; set; }
        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                if (_content == value) return;
                _content = value;
                OnPropertyChanged();
            }
        }
        private Guid _issuerId;
        public Guid IssuerId
        {
            get => _issuerId;
            set
            {
                if (_issuerId == value) return;
                _issuerId = value;
                OnPropertyChanged();
            }
        }
        private DateTime _issuedOn;
        public DateTime IssuedOn
        {
            get => _issuedOn;
            set
            {
                if (_issuedOn == value) return;
                _issuedOn = value;
                OnPropertyChanged();
            }
        }
        private string _issuedFor;
        public string IssuedFor
        {
            get => _issuedFor;
            set
            {
                if (_issuedFor == value) return;
                _issuedFor = value;
                OnPropertyChanged();
            }
        }
        private DateTime? _expireDate;
        public DateTime? ExpireDate
        {
            get => _expireDate;
            set
            {
                if (_expireDate == value) return;
                _expireDate = value;
                OnPropertyChanged();
            }
        }
        private string _isActive;
        public string IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private Guid _userId;
        public Guid UserId
        {
            get => _userId;
            set
            {
                if (_userId == value) return;
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _policy;
        public string Policy
        {
            get => _policy;
            set
            {
                if (_policy == value) return;
                _policy = value;
                OnPropertyChanged();
            }
        }


        [ForeignKey(nameof(UserId))]
        public virtual TeramUser User { get; set; }
        [ForeignKey(nameof(IssuerId))]
        public virtual TeramUser Issuer { get; set; }

    }
}


