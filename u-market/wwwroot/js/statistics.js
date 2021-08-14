const width = 1000;
const height = 600;
const margin = { 'top': 20, 'right': 20, 'bottom': 100, 'left': 100 };
const graphWidth = width - margin.left - margin.right;
const graphHeight = height - margin.top - margin.bottom;
const colors = [
    "#FF0000",
    "#FF1100",
    "#FF2200",
    "#FF3300",
    "#FF4400",
    "#FF5500",
    "#FF6600",
    "#FF7700",
    "#FF8800",
    "#FF9900",
    "#FFAA00",
    "#FFBB00",
    "#FFCC00",
    "#FFDD00",
    "#FFEE00",
    "#FFFF00",
    "#EEFF00",
    "#DDFF00",
    "#CCFF00",
    "#BBFF00",
    "#AAFF00",
    "#99FF00",
    "#88FF00",
    "#77FF00",
    "#66FF00",
    "#55FF00",
    "#44FF00",
    "#33FF00",
    "#22FF00",
    "#11FF00",
    "#00FF00"
]

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

const getColor = (maxAmount, amount) => {
    const colorIndex = Math.trunc(amount / (maxAmount / (colors.length - 1)))
    return colors[colorIndex];
}

const getData = async () => {
    return $.get("/Statistics/GetAll", (data, status) => {
        if (status != "success") {
            alert("error ocured while trying to get the statistics");
        }
    });
};

getData().then((result) => {
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
});