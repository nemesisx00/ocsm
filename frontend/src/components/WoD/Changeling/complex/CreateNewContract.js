import './CreateNewContract.css'
import React from 'react'

export default class CreateNewContract extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			label: ''
		}
	}
	
	render()
	{
		return (
			<div className={`createNewContract ${this.props.className}`}>
				<div className="row">
					<div className="label">Label:</div>
					<input type="text" value={this.state.label} onChange={(e) => this.changeValue(e.target.value)} />
				</div>
				<div className="row center">
					<button onClick={() => this.props.createHandler(this.state.label)}>Create</button>
				</div>
			</div>
		)
	}
	
	changeValue(value)
	{
		this.setState(() => {
			return { label: value }
		})
	}
}
