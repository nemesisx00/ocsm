import React from 'react'
import CheckBox from '../CheckBox'
import './GlamourTracker.css'

const MaxRow = 10
const WyrdGlamourIntervals = Object.freeze({
	1: 10,
	2: 11,
	3: 12,
	4: 13,
	5: 14,
	6: 15,
	7: 20,
	8: 30,
	9: 50,
	10: 100
})

export default class GlamourTracker extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			max: WyrdGlamourIntervals[props.wyrd ? props.wyrd : 1],
			spent: 0,
			unspent: 0
		}
	}
	
	render()
	{
		//TODO: figure out how to update the spent/unspent state dynamically
		
		let rows = []
		let boxes = []
		for(let i = 0; i < this.state.max; i++)
		{
			if(i % MaxRow === 0)
			{
				if(boxes.length > 0)
					rows.push((<div className="boxLine" key={`glamourRow-${boxes.length}`}>{boxes}</div>))
				boxes = []
			}
			boxes.push(<CheckBox key={`glamour-${i}`} />);
		}
		//Catch the last row of boxes
		if(boxes.length > 0)
			rows.push((<div className="boxLine" key={`glamourRow-${boxes.length}`}>{boxes}</div>))
		
		return (
			<div className="glamourTracker">
				<p className="label">Glamour</p>
				{rows}
			</div>
		)
	}
}
