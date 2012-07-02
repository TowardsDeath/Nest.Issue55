using System;
using System.Configuration;

using Nest.Issue55.Web.Models;

namespace Nest.Issue55.Web.Services
{
    public class ElasticSearchService
    {
        protected readonly ElasticClient ElasticSearch;

        public ElasticSearchService()
        {
            var connection = new ConnectionSettings( new Uri( ConfigurationManager.AppSettings[ "ElasticUri" ] ) );
            connection.SetDefaultIndex( "products" );
            this.ElasticSearch = new ElasticClient( connection );
        }

        public void CreateIndex()
        {
            if (this.ElasticSearch.IndexExists("products").Exists)
                this.ElasticSearch.DeleteIndex("products");

            this.ElasticSearch.CreateIndex("products", new IndexSettings 
            {
                NumberOfReplicas = 1,
                NumberOfShards = 5
            });
        }

        public void IndexProduct( ElasticSearchProduct p )
        {
            this.ElasticSearch.Map<ElasticSearchProduct>();
            this.ElasticSearch.Index<ElasticSearchProduct>( p );
        }

        public Result PerformSearch()
        {
            var search = new SearchDescriptor<ElasticSearchProduct>()
                .MatchAll()
                .FacetTerm( "format", f => f.OnField( p => p.Format ) );

            var response = this.ElasticSearch.Search<ElasticSearchProduct>( s => search );
            return this.FormatResult( response );
        }

        protected Result FormatResult( QueryResponse<ElasticSearchProduct> response )
        {
            var result = new Result
            {
                Products = response.Documents
            };
            this.FormatFacets( response, result );

            return result;
        }

        protected Result FormatFacets( QueryResponse<ElasticSearchProduct> response, Result result )
        {
            if( response.Facets == null ) return result;

            foreach( var facet in response.Facets )
            {
                if( !( facet.Value is TermFacet ) ) continue;

                var f = facet.Value as TermFacet;
                foreach( var value in f.Items )
                {
                    result.AddFacetField( facet.Key, value.Term, value.Count );
                }
            }
            return result;
        }
    }
}