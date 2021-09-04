const width = 1000;
const height = 600;
const margin = { 'top': 20, 'right': 20, 'bottom': 100, 'left': 100 };
const graphWidth = width - margin.left - margin.right;
const graphHeight = height - margin.top - margin.bottom;

let currGraph = "productAmountGraphContainer";
$("#changeGraph").change(() => {
    if (currGraph == "productAmountGraphContainer") {
        document.getElementById("productAmountGraphContainer").hidden = true;
        document.getElementById("productAmountPieContainer").hidden = false;
        currGraph = "productAmountPieContainer";
    } else {
        document.getElementById("productAmountGraphContainer").hidden = false;
        document.getElementById("productAmountPieContainer").hidden = true;
        currGraph = "productAmountGraphContainer";
    }
})

$("#graphDataType").change(() => {
    drawGraph($("#graphDataType").val());
});


const drawBarGraph = (data) => {
    d3.select("#productAmountGraphContainer").select("svg").remove();

    const svg = d3.select('#productAmountGraphContainer')
        .append('svg')
        .attr('width', width)
        .attr('height', height);

    const graph = svg.append('g')
        .attr('width', graphWidth)
        .attr('height', graphHeight)
        .attr('transform', `translate(${margin.left}, ${margin.top})`);

    const gXAxis = graph.append('g')
        .attr('transform', `translate(0, ${graphHeight})`);

    const gYAxis = graph.append('g')

    const y = d3.scaleLinear()
        .domain([0, d3.max(data, d => d.Amount)])
        .range([graphHeight, 0]);

    const x = d3.scaleBand()
        .domain(data.map(item => item.product))
        .range([0, 500])
        .paddingInner(0.2)
        .paddingOuter(0.2);

    const rects = graph.selectAll('rect')
        .data(data);

    rects.enter()
        .append('rect')
        .attr('class', 'bar-rect')
        .attr('width', x.bandwidth)
        .attr('height', d => graphHeight - y(d.Amount))
        .attr('fill', d => getColor(d3.max(data, d => d.Amount), d.Amount))
        .attr('rx', 7)
        .attr('ry', 7)
        .attr('x', d => x(d.product))
        .attr('y', d => y(d.Amount));

    const xAxis = d3.axisBottom(x);

    const yAxis = d3.axisLeft(y)
        .ticks(5);

    gXAxis.call(xAxis);

    gYAxis.call(yAxis);

    gXAxis.selectAll('text')
        .style('font-size', 14);

    gYAxis.selectAll('text')
        .style('font-size', 14);
}

const drawPieGraph = (data) => {
    d3.select("#productAmountPieContainer").select("svg").remove();
    const pieSvg = d3.select("#productAmountPieContainer")
        .append('svg')
        .attr('width', width)
        .attr('height', height);
    const radius = Math.min(graphWidth, graphHeight) / 2;
    const g = pieSvg.append("g").attr("transform", "translate(" + graphWidth / 2 + "," + height / 2 + ")");

    // Generate the pie
    const pie = d3.pie().value(d => d.Amount);

    // Generate the arcs
    const arc = d3.arc()
        .innerRadius(0)
        .outerRadius(radius);

    const path = d3.arc()
        .outerRadius(radius - 10)
        .innerRadius(0);

    const label = d3.arc()
        .outerRadius(radius)
        .innerRadius(radius - 80);

    const arcs = g.selectAll("arc")
        .data(pie(data))
        .enter()
        .append("g")
        .attr("class", "arc");

    arcs.append("path")
        .attr("d", path)
        .attr('fill', d => getColor(d3.max(data, d => d.Amount), d.data.Amount));

    arcs.append("text")
        .attr("transform", d => "translate(" + label.centroid(d) + ")")
        .text(d => d.data.product + " " + d.data.Amount);
}

const getData = async (dataType) => {
    if (dataType == "store") {
        return $.get("/Statistics/getPurchasesByStore", (data, status) => {
            if (status != "success") {
                alert("error ocured while trying to get the statistics");
            }
        });
    } else if (dataType == "product") {
        return $.get("/Statistics/getPurchasesByProduct", (data, status) => {
            if (status != "success") {
                alert("error ocured while trying to get the statistics");
            }
        });
    }
    
};

const drawGraph = (dataType) => {
    getData(dataType).then((result) => {
        let jsonData = JSON.parse(result);
        let data = [];
        for (o in jsonData) {
            const obj = {
                product: "",
                Amount: 0
            }
            obj.product = o;
            obj.Amount = jsonData[o];
            data.push(obj);
        }
        drawBarGraph(data);
        drawPieGraph(data);

    });
}

$(document).ready(() => {
	document.getElementById("productAmountPieContainer").hidden = true;
    drawGraph("store");
})
