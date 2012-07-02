using System.Collections.Generic;

namespace Nest.Issue55.Web.Models
{
    public class Result
    {
        public IEnumerable<ElasticSearchProduct> Products { get; set; }
        public IDictionary<string, ICollection<KeyValuePair<string, int>>> FieldFacets { get; set; }

        public Result()
        {
            this.Products = new List<ElasticSearchProduct>();
            this.FieldFacets = new Dictionary<string, ICollection<KeyValuePair<string, int>>>();
        }

        public void AddFacetField( string facetName, string value, int count )
        {
            if( !this.FieldFacets.ContainsKey( facetName ) )
            {
                this.FieldFacets.Add( facetName, new List<KeyValuePair<string, int>>() );
            }

            this.FieldFacets[ facetName ].Add( new KeyValuePair<string, int>( value, count ) );
        }
    }
}