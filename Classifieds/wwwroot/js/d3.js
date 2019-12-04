//Advert Status Graph
d3.json("/Admin/CountAdvertByStatus").then(function (data) {
    var margin = 30;
    var svgWidth = 240;
    var svgHeight = 250;
    var minRadius = 20;
    var arcWidth = 25;
    var arcPadding = 5;
    var dataSet = new Array(data.length);
    var domain = new Array(data.length);

    for (i = 0; i < data.length; i++) {
        dataSet[i] = data[i].percent;
        domain[i] = data[i].column;
    }

    var toolTip = d3.select("#ad-status-graph .card-body")
        .append("div")
        .attr("class", "tooltip")
        .style("opacity", 0);

    //var color = d3.scaleOrdinal(d3.schemePastel2);

    var color = ["#fdae61", "#f46d43", "#d53e4f"];


    var svg = d3.select("#ad-status-graph .card-body")
        .append("svg")
        .attr("width", svgWidth)
        .attr("height", svgHeight)
        .append("g")
        .attr("transform", function () {
            var translate = [svgWidth / 2, svgHeight / 2];
            return "translate(" + translate + ")";
        });

    var arcGenerator = d3.arc()
        .innerRadius(function (d, i) {
            return minRadius + i * arcWidth + arcPadding;
        })
        .outerRadius(function (d, i) {
            return minRadius + (i + 1) * arcWidth;
        })
        .startAngle(0 * Math.PI / 180)
        .endAngle(function (d, i) {
            return d * 5 * Math.PI / 180;
        });


    var path = svg.selectAll("path")
        .data(dataSet)
        .enter()
        .append("path")
        .attr("d", arcGenerator)
        .attr("fill", function (d, i) { return color[i];})
        .on("mouseover", function (d, i) {
            toolTip.style("opacity", 0.9);
            toolTip.html(domain[i] + "( " + d + "% )")
                .style("left", svgWidth/2 -20 + "px")
                .style("top", (svgHeight + 25) + "px")
                .style("color", "#666");
        })
        .on("mouseout", function (d, i) {
            toolTip.style("opacity", 0);
        });

});
//Advert Location Pie Chart
d3.json("/Admin/CountAdvertByLocation").then(function (data) {
    var svgWidth = 500;
    var svgHeight = 250;
    var innerRadius = 40;
    var outerRadius = 100;
    // The radius of the pieplot is half the width or half the height (smallest one). I subtract a bit of margin.
    var radius = Math.min(svgWidth, svgHeight) / 2;

    var dataSet = new Array(data.length);
    var domain = new Array(data.length);

    for (i = 0; i < data.length; i++) {
        dataSet[i] = data[i].percent;
        domain[i] = data[i].column;
    }

    var r = 0;
    var colors = ["#6e40aa", "#963db3", "#bf3caf", "#e4419d", "#fe4b83",
        "#ff5e63", "#ff7847", "#fb9633", "#e2b72f", "#c6d63c", "#aff05b"];

    var svg = d3.select("#ad-location-pie .card-body")
        .append("svg")
        .attr("width", svgWidth)
        .attr("height", svgHeight)
        .append("g")
        .attr("transform", function () {
            var translate = [svgWidth/2, svgHeight / 2];
            return "translate(" + translate + ")";
        });



    //Generate Arcs
    var arc = d3.arc()
        .innerRadius(innerRadius)
        .outerRadius(outerRadius);

    // Another arc that won't be drawn. Just for labels positioning
    var outerArc = d3.arc()
        .innerRadius(outerRadius * 0.9)
        .outerRadius(outerRadius * 0.9);

    //Generate Pie
    var pie = d3.pie().sort(null);

    //Generate Groups
    var arcs = svg.selectAll("arc")
        .data(pie(dataSet))
        .enter()
        .append("g")
        .attr("class", "arc");


    //Draw arc paths
    arcs.append("path")
        .attr("fill", function (d, i) {
            return colors[i];
        })
        .attr("d", arc);

    // Add the polylines between chart and labels:
    svg
        .selectAll('allPolylines')
        .data(pie(dataSet))
        .enter()
        .append('polyline')
        .attr("stroke", "#999")
        .style("fill", "none")
        .attr("stroke-width", 1)
        .attr('points', function (d) {
            var posA = arc.centroid(d); // line insertion in the slice
            var posB = outerArc.centroid(d); // line break: we use the other arc generator that has been built only for that
            var posC = outerArc.centroid(d); // Label position = almost the same as posB
            var midangle = d.startAngle + (d.endAngle - d.startAngle) / 2 // we need the angle to see if the X position will be at the extreme right or extreme left
            posC[0] = radius * 0.95 * (midangle < Math.PI ? 1 : -1); // multiply by 1 or -1 to put it on the right or on the left
            return [posA, posB, posC];
        });

    // Add the polylines between chart and labels:
    svg
        .selectAll('allLabels')
        .data(pie(dataSet))
        .enter()
        .append('text')
        .text(function (d,i)
        {
            //console.log(d.data.column);
            return domain[i];
        })
        .style("fill", "#666")
        .attr("font-size", "10px")
        .attr('transform', function (d) {
            var pos = outerArc.centroid(d);
            var midangle = d.startAngle + (d.endAngle - d.startAngle) / 2;
            pos[0] = radius * 0.99 * (midangle < Math.PI ? 1 : -1);
            return 'translate(' + pos + ')';
        })
        .style('text-anchor', function (d) {
            var midangle = d.startAngle + (d.endAngle - d.startAngle) / 2;
            return (midangle < Math.PI ? 'start' : 'end');
        });
});