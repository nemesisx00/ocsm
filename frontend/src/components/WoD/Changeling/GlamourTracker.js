import React from 'react'
import CheckBox from '../CheckBox'
import './GlamourTracker.css'

const MaxRow = 10

export default class GlamourTracker extends React.Component
{
	render()
	{
		//TODO: figure out how to update the spent/unspent state dynamically
		
		let rows = []
		let boxes = []
		for(let i = 0; i < this.props.max; i++)
		{
			if(i % MaxRow === 0)
			{
				if(boxes.length > 0)
					rows.push((<div className="boxLine" key={`glamourRow-${boxes.length}`} >{boxes}</div>))
				boxes = []
			}
			boxes.push(<CheckBox key={`glamour-${i}`} checked={this.props.spent > i} clickHandler={() => this.props.changeHandler(i + 1)} />);
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
