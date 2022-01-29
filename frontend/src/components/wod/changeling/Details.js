import '../Details.css'
import React from 'react'

export default class Details extends React.Component
{
	render()
	{
		return (
			<div className="details">
				<div className="column">
					<div className="label">Name:</div>
					<div className="label">Player:</div>
					<div className="label">Chronicle:</div>
				</div>
				<div className="column">
					<input type="text" value={this.props.details.name} onChange={(e) => this.props.changeHandler({name: e.target.value})} />
					<input type="text" value={this.props.details.player} onChange={(e) => this.props.changeHandler({player: e.target.value})} />
					<input type="text" value={this.props.details.chronicle} onChange={(e) => this.props.changeHandler({chronicle: e.target.value})} />
				</div>
				<div className="column">
					<div className="label">Concept:</div>
					<div className="label">Virtue:</div>
					<div className="label">Vice:</div>
				</div>
				<div className="column">
					<input type="text" value={this.props.details.concept} onChange={(e) => this.props.changeHandler({concept: e.target.value})} />
					<input type="text" value={this.props.details.virtue} onChange={(e) => this.props.changeHandler({virtue: e.target.value})} />
					<input type="text" value={this.props.details.vice} onChange={(e) => this.props.changeHandler({vice: e.target.value})} />
				</div>
				<div className="column">
					<div className="label">Seeming:</div>
					<div className="label">Kith:</div>
					<div className="label">Court:</div>
				</div>
				<div className="column">
					<input type="text" value={this.props.details.seeming} onChange={(e) => this.props.changeHandler({seeming: e.target.value})} />
					<input type="text" value={this.props.details.kith} onChange={(e) => this.props.changeHandler({kith: e.target.value})} />
					<input type="text" value={this.props.details.court} onChange={(e) => this.props.changeHandler({court: e.target.value})} />
				</div>
			</div>
		)
	}
}
