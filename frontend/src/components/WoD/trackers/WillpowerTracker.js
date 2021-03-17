import React from 'react'
import CheckBox from '../CheckBox'
import './WillpowerTracker.css'

export default class WillpowerTracker extends React.Component
{
	render()
	{
		let boxes = []
		for(let i = 0; i < this.props.max; i++)
		{
			boxes.push(<CheckBox key={`willpower-${i}`} checked={this.props.spent > i} clickHandler={() => this.props.changeHandler(i + 1)}/>);
		}
		
		return (
			<div className="willpowerTracker">
				<p className="label">Willpower</p>
				<div className="boxLine">
					{boxes}
				</div>
			</div>
		)
	}
}
