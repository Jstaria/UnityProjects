import folium
import geopandas as gpd
import requests

# Define the area of interest (South Florida)
south_florida_boundaries = {
    'south': 25.0,
    'north': 27.0,
    'west': -81.5,
    'east': -80.0
}

# Create a base map
m = folium.Map(location=[26.0, -80.5], zoom_start=7)

# Load a shapefile or GeoJSON that contains flood zones (replace with actual path)
# For this example, we're assuming a GeoJSON file with flood zones.
# You can download flood zone data from sources like FEMA.
flood_zones_url = "https://example.com/flood_zones.geojson"  # Replace with a valid URL
flood_zones_data = requests.get(flood_zones_url).json()

# Create a GeoDataFrame from the flood zones data
gdf_flood_zones = gpd.GeoDataFrame.from_features(flood_zones_data['features'])

# Filter for South Florida
gdf_south_florida = gdf_flood_zones.cx[south_florida_boundaries['west']:south_florida_boundaries['east'],
                                        south_florida_boundaries['south']:south_florida_boundaries['north']]

# Add flood zones to the map
folium.GeoJson(
    gdf_south_florida,
    name='Flood Zones',
    style_function=lambda x: {
        'color': 'blue',
        'weight': 1,
        'fillOpacity': 0.5,
        'fillColor': 'blue' if x['properties']['zone_type'] == 'AE' else 'red'  # Example zone type condition
    }
).add_to(m)

# Add a layer control
folium.LayerControl().add_to(m)

# Save the map to an HTML file
m.save('south_florida_flood_zones.html')

print("Map has been created and saved as 'south_florida_flood_zones.html'")
