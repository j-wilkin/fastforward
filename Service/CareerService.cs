using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fastforward.Models;

namespace fastforward.Service
{
    public static class CareerService
    {
        public static List<RelatedOccupation> GetRelatedOccupations(FastForwardContext context, int careerId)
        {
            return context.RelatedOccupations.Where(x => x.CareerId == careerId).ToList();
        }

        public static List<RelatedOccupation> GetRelatedOccupations(int careerId)
        {
            using (var context = new FastForwardContext())
            {
                return GetRelatedOccupations(context, careerId);
            }
        }
    }
}