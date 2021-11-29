import '../Details.css'
import React from 'react'

export default class Details extends React.Component
{
	render()
	{
		return (
			<div className="details">
				<div className="column">
					<div className="label">Shadow Name:</div>
					<div className="label">Player:</div>
					<div className="label">Chronicle:</div>
				</div>
				<div className="column">
					<input type="text" value={this.props.details.shadowName} onChange={(e) => this.props.changeHandler({shadowName: e.target.value})} />
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
					<div className="label">Path:</div>
					<div className="label">Order:</div>
					<div className="label">Legacy:</div>
				</div>
				<div className="column">
					<input type="text" value={this.props.details.path} onChange={(e) => this.props.changeHandler({path: e.target.value})} />
					<input type="text" value={this.props.details.order} onChange={(e) => this.props.changeHandler({order: e.target.value})} />
					<input type="text" value={this.props.details.legacy} onChange={(e) => this.props.changeHandler({legacy: e.target.value})} />
				</div>
			</div>
		)
	}
}
