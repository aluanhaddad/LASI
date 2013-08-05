﻿using LASI.Algorithm.ClauseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LASI.Utilities;

namespace LASI.Algorithm.Analysis.Binders.Experimental
{
    class NestableClause : Clause, INestableLexical<NestableClause>
    {
        public NestableClause(IEnumerable<INestableLexical<NestableClause>> constituentClauses, NestableClause parent = null)
            : base(from clause in constituentClauses from phrase in clause.Self.Phrases select phrase) {
            Parent = parent;
            Children = constituentClauses;
        }
        public IEnumerable<NestableClause> GetClausesSkippingSubordinates() {
            var temp = Children.TakeWhile(c => !(c is SubordinateClause));
            var bookend = Children.Skip(temp.Count() + 1).Take(1);
            var result = temp.Take(temp.Count() - 1).Concat(new[] { new NestableClause(new[] { temp.Last(), bookend.FirstOrDefault() }), this });
            return result.Select(c => c.Self);
        }


        #region Properties

        public NestableClause Parent { get; set; }
        public IEnumerable<INestableLexical<NestableClause>> Children { get; set; }
        public NestableClause Self { get { return this; } }

        #endregion

    }

}