using ContentModeratorSDK.Service.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Tests.Helpers
{
    public class TermMatchComparer : IEqualityComparer<MatchTerm>
    {
        public bool Equals(MatchTerm x, MatchTerm y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) return false;

            return x.Index == y.Index && x.Term == y.Term;
        }

        public int GetHashCode(MatchTerm obj)
        {
            if (object.ReferenceEquals(obj, null)) return 0;

            int hashCodeIndex = obj.Index.GetHashCode();
            int hasCodeTerm = obj.Term.GetHashCode();

            return hashCodeIndex ^ hasCodeTerm;
        }
    }

    public class URLMatchComparer : IEqualityComparer<MatchUrl>
    {
        public bool Equals(MatchUrl x, MatchUrl y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) return false;

            return x.URL == y.URL 
                && x.categories.Adult == y.categories.Adult
                && x.categories.Malware == y.categories.Malware
                && x.categories.Phishing == y.categories.Phishing;
        }

        public int GetHashCode(MatchUrl obj)
        {
            if (object.ReferenceEquals(obj, null)) return 0;

            int hashCodeIndex = obj.URL.GetHashCode();
            int hasCodeTerm = obj.categories.GetHashCode();

            return hashCodeIndex ^ hasCodeTerm;
        }
    }
}
