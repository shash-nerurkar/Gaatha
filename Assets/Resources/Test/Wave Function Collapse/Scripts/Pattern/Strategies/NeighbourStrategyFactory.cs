using System;
using System.Collections.Generic;
using System.Reflection;

public class NeighbourStrategyFactory
{
    Dictionary<string, Type> strategies;

    public NeighbourStrategyFactory() {
        strategies = new Dictionary<string, Type>();

        Type[] allTypesInAssembly = Assembly.GetExecutingAssembly().GetTypes();
        foreach( Type type in allTypesInAssembly )
            if( type.GetInterface( typeof( IFindNeighbourStrategy ).ToString() ) != null )
                strategies.Add( type.Name.ToLower(), type );
    }

    internal IFindNeighbourStrategy CreateInstance( string strategyName ) {
        Type type = GetTypeToCreate( strategyName );
        if( type == null )
            type = GetTypeToCreate( "overlap" );
        
        return Activator.CreateInstance(type) as IFindNeighbourStrategy;
    }

    private Type GetTypeToCreate( string strategyName ) {
        foreach ( KeyValuePair<string, Type> strategyPair in strategies )
            if( strategyPair.Key.Contains( strategyName ) )
                return strategyPair.Value;
        
        return null;
    }
}
